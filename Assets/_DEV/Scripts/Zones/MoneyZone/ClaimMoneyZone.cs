using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using DEV.Scripts.Managers;
using DEV.Scripts.Interface;
using DEV.Scripts.Signals;
using DEV.Scripts.Passenger;

namespace DEV.Scripts.Zones.MoneyZone
{
    public class ClaimMoneyZone : MonoBehaviour, IInteractZone
    {
        [SerializeField] private GameObject moneyPrefab;

        [SerializeField] private Transform moneyFirstTargetPoint;
        [SerializeField] private Transform moneySpawnPoint;

        [Space]
        [SerializeField] private uint maxZMoneyCount;
        [SerializeField] private uint maxXMoneyCount;

        [SerializeField] private float moneyYSpawnHeight;

        private List<GameObject> _moneyObjects = new List<GameObject>();

        private uint _moneyPerObject;
        private uint _totalMoney;

        private void Start() => _moneyPerObject = Passengers.Instance.PricePerSuitcase;

        private void OnEnable() => EventBus.CorePassengerSignals.OnCheckedPassengerTicket += OnMoneyEarned;

        private void OnDisable() => EventBus.CorePassengerSignals.OnCheckedPassengerTicket -= OnMoneyEarned;

        public void Enter() => ClaimMoney();

        public void Leave() { }

        public void Finish() { }

        public void Tick(float dt) { }

        private void OnMoneyEarned(uint moneyPrice)
        {
            _totalMoney += moneyPrice;
            SpawnMoneyObjects(moneyPrice);
        }

        private void SpawnMoneyObjects(uint moneyPrice)
        {
            uint moneyCount = moneyPrice / _moneyPerObject;

            for (int i = 0; i < moneyCount; i++)
            {
                Vector3 spawnPosition = moneySpawnPoint.position
                                        + Vector3.up * moneyYSpawnHeight;
                var moneyObject = Instantiate(moneyPrefab, spawnPosition, Quaternion.identity,moneyFirstTargetPoint);
                _moneyObjects.Add(moneyObject);

                int index = _moneyObjects.Count - 1;
                Vector3 targetPos = GetMoneyTargetPosition(index);

                moneyObject.transform.DOJump(targetPos, 1f, 1, .5f)
                           .SetEase(Ease.Linear);
            }
        }

        private void ClaimMoney()
        {
            foreach (var moneyObject in _moneyObjects)
                Destroy(moneyObject);

            GameManager.Instance.AddMoney(_totalMoney);

            _moneyObjects.Clear();
            _totalMoney = 0;
        }

        private Vector3 GetMoneyTargetPosition(int index)
        {
            if (!moneyFirstTargetPoint)
                return moneySpawnPoint ? moneySpawnPoint.position : Vector3.zero;

            uint perLayer = maxXMoneyCount * maxZMoneyCount;
            if (perLayer == 0) perLayer = 1;

            uint idx = (uint)index;
            uint layer = idx / perLayer;
            uint inLayer = idx % perLayer;

            uint xIndex = maxXMoneyCount > 0 ? inLayer % maxXMoneyCount : 0;
            uint zIndex = maxXMoneyCount > 0 ? inLayer / maxXMoneyCount : 0;

            float xOffset = xIndex;
            float zOffset = zIndex;
            float yOffset = layer * moneyYSpawnHeight;

            Vector3 basePos = moneyFirstTargetPoint.position;
            return basePos + new Vector3(xOffset, yOffset, zOffset);
        }
    }
}