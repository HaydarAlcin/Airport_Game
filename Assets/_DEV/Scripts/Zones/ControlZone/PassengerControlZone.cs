using UnityEngine;
using System.Collections.Generic;
using DEV.Scripts.Player;
using DEV.Scripts.Signals;
using DEV.Scripts.SO.Zone.ControlZone;

namespace DEV.Scripts.Zones.ControlZone
{
    internal class PassengerControlZone : ControlZone
    {
        private PlayerStackController _playerStackController;

        private readonly Stack<Transform> _currentSuitcases = new();

        protected override void SetSettings()
        {
            var passengerZoneData = zoneData as ControlZoneData_SO;

            _currentPassengerState = passengerZoneData.CurrentPassengerState;
            _nextPassengerState = passengerZoneData.NextPassengerState;
            SpacingValue = passengerZoneData.WaitingSpacing;

            _playerStackController = EventBus.CorePlayerSignals.PlayerManager?.Invoke().GetPlayerStackController();
        }

        public override void Leave()
        {
            Active = false;
            SetCircleImageColor(zoneData.InactiveColor);
        }

        public override void Tick(float dt)
        {
            if (!Active || _playerStackController == null) return;

            if (!_currentPassenger) return;

            if (_currentPassenger.IsPassengerMoving()) return;

            if (_currentSuitcases.Count == 0)
            {
                CompleteCurrentPassenger();
                return;
            }

            Timer += dt;
            if (Timer < _playerStackController.TakeItemDelay) return;
            Timer -= _playerStackController.TakeItemDelay;

            var item = _currentSuitcases.Peek();
            if (!item) { _currentSuitcases.Pop(); return; }

            if (_playerStackController.TryCollect(item))
            {
                _currentSuitcases.Pop();
                _currentPassenger.OnSuitcaseCollected(item);
            }
        }

        internal override void TryAdvancePassenger()
        {
            while (_passengers.Count > 0)
            {
                var p = _passengers.Peek();
                if (!p)
                {
                    _passengers.Dequeue();
                    continue;
                }

                var list = p.GetCollectableSuitcases();

                if (list == null || list.Count == 0) return;

                for (var i = 0; i < list.Count; i++)
                {
                    var suitcase = list[i];
                    _currentSuitcases.Push(suitcase);
                }

                _currentPassenger = p;
                return;
            }
        }
    }
}