using DEV.Scripts.Enums;
using DEV.Scripts.Passenger;
using DEV.Scripts.Signals;
using System.Collections.Generic;
using UnityEngine;

namespace DEV.Scripts.Zones.ControlZone
{
    internal class ControlZone : BaseZone
    {
        [Space]
        [SerializeField] internal ControlZonePathSet pathSet;

        protected PassengerManager _currentPassenger;

        protected PassengerStatus _currentPassengerState;
        protected PassengerStatus _nextPassengerState;

        protected readonly Queue<PassengerManager> _passengers = new();

        protected virtual void OnEnable() => EventBus.CorePassengerSignals.OnChangedPassengerState += OnPassengerStateChanged;

        protected virtual void OnDisable() => EventBus.CorePassengerSignals.OnChangedPassengerState -= OnPassengerStateChanged;

        public override void Enter()
        {
            base.Enter();
            if (!_currentPassenger)
                TryAdvancePassenger();
        }

        internal virtual void OnPassengerStateChanged(PassengerStatus state, Transform passenger)
        {
            if (state == _currentPassengerState)
            {
                var p = passenger.GetComponent<PassengerManager>();
                EnqueuePassenger(p);
            }
        }

        internal virtual void EnqueuePassenger(PassengerManager p)
        {
            if (!p) return;

            _passengers.Enqueue(p);

            int index = _passengers.Count - 1;
            var steps = pathSet.Steps;
            var target = GetWaitingAnchor(index);
            p.SetPath(steps, target, OnPassengerPathCompleted);
        }

        internal virtual Vector3 GetWaitingAnchor(int index)
        {
            if (!pathSet || !pathSet.FirstWaitingPoint)
                return Vector3.zero;

            var root = pathSet.FirstWaitingPoint;
            var localOffset = new Vector3(0f, 0f, -index * SpacingValue);

            return root.TransformPoint(localOffset);
        }

        internal virtual void ReassignWaitingAnchors()
        {
            if (_passengers.Count == 0) return;

            int i = 0;
            foreach (var p in _passengers)
            {
                p.SetTargetWaitingPos(GetWaitingAnchor(i));
                i++;
            }
        }

        internal virtual void CompleteCurrentPassenger()
        {
            _currentPassenger?.SwitchState(_nextPassengerState);
            _currentPassenger = null;

            _passengers.Dequeue();

            TryAdvancePassenger();
            ReassignWaitingAnchors();
        }

        internal virtual void OnPassengerPathCompleted() { }

        internal virtual void TryAdvancePassenger() { }
    }
}