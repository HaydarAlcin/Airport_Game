using UnityEngine;
using DEV.Scripts.Managers;
using DEV.Scripts.SO.Zone.MoneyZone;
using DEV.Scripts.Interface;
using DEV.Scripts.Signals;

namespace DEV.Scripts.Zones.MoneyZone
{
    internal class SpentMoneyZone : MonoBehaviour, IInteractZone
    {
        [SerializeField] private MoneyZoneData_SO data;

        [SerializeField] private MoneyZoneVisualController visualController;

        private BoxCollider _collider;

        private bool _active;
        private uint _paid;

        private void Start()
        {
            bool canAfford = GameManager.Instance.TotalMoney > data.TotalCost;

            _collider = GetComponent<BoxCollider>();
            _collider.enabled = canAfford;
            visualController.SetFillAmount(0f);
            visualController.SetCostText(data.TotalCost);
            visualController.SetOpenSignActive(canAfford);
        }

        private void OnEnable() => EventBus.CoreGameSignals.OnMoneyEarned += OnMoneyEarned;

        private void OnDisable() => EventBus.CoreGameSignals.OnMoneyEarned -= OnMoneyEarned;

        public void Enter()
        {
            _active = true;
            visualController.PlayMoneyParticle();
            visualController.SetOpenSignActive(false);
        }

        public void Tick(float dt)
        {
            if (!_active || _paid >= data.TotalCost) return;

            uint want = 0;
            if (data.DrainPerSec > 0f)
                want = (uint)Mathf.CeilToInt(data.DrainPerSec * dt);

            if (want < data.MinChunk) want = data.MinChunk;

            uint remaining = data.TotalCost - _paid;
            if (want > remaining) want = remaining;

            uint taken = GameManager.Instance.Withdraw(want);
            if (taken == 0) return;

            _paid += taken;

            visualController.SetFillAmount((float)_paid / data.TotalCost);
            visualController.SetCostText(data.TotalCost - _paid);

            if (_paid >= data.TotalCost)
            {
                Finish();
            }
        }

        public void Leave()
        {
            _active = false;
            visualController.StopMoneyParticle();
        }

        public void Finish()
        {
            _active = false;
            visualController.StopMoneyParticle();

            EventBus.CoreGameSignals.OnMoneyZoneFinished?.Invoke(data.UnlockType);
            this.gameObject.SetActive(false);
        }

        private void OnMoneyEarned(uint totalMoney)
        {
            if (totalMoney < data.TotalCost - _paid) return;
            _collider.enabled = true;
            visualController.SetOpenSignActive(true);
        }
    }
}