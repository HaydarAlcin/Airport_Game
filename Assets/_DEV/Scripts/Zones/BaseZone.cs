using UnityEngine;
using DEV.Scripts.Interface;
using DEV.Scripts.SO.Zone;

namespace DEV.Scripts.Zones
{
    internal class BaseZone : MonoBehaviour, IInteractZone
    {
        [SerializeField] protected BaseControlZoneData_SO zoneData;
        [Space]
        [SerializeField] private ZoneVisualController visualController;

        private bool _active;
        private float _timer;
        private float _waitingSpacing;
        private float _checkDelay;

        public bool Active { get => _active; set => _active = value; }
        public float Timer { get => _timer; set => _timer = value; }
        public float SpacingValue { get => _waitingSpacing; set => _waitingSpacing = value; }
        public float CheckDelay { get => _checkDelay; set => _checkDelay = value; }

        private void Start() => SetSettings();

        protected virtual void SetSettings() { }

        public virtual void Enter()
        {
            Active = true;
            Timer = 0f;
            SetCircleImageColor(zoneData.ActiveColor);
        }

        public virtual void Leave() { }

        public virtual void Finish() { }

        public virtual void Tick(float dt) { }

        protected virtual void SetCircleImageColor(Color32 color) => visualController.SetCircleImageColor(color);
    }
}