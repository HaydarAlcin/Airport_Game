using UnityEngine;
using DEV.Scripts.Passenger;
using DEV.Scripts.Signals;
using DEV.Scripts.SO.Zone.ControlZone;

namespace DEV.Scripts.Zones.ControlZone
{
    internal class TicketControlZone : ControlZone
    {
        protected override void SetSettings()
        {
            var ticketControlZoneData = zoneData as ControlZoneData_SO;
            _currentPassengerState = ticketControlZoneData.CurrentPassengerState;
            _nextPassengerState = ticketControlZoneData.NextPassengerState;
            SpacingValue = ticketControlZoneData.WaitingSpacing;
            CheckDelay = ticketControlZoneData.CheckDelay;
        }

        public override void Leave()
        {
            Active = false;
            SetCircleImageColor(zoneData.InactiveColor);
        }

        public override void Tick(float dt)
        {
            if (!Active) return;

            if (!_currentPassenger)
            {
                TryAdvancePassenger();
                if (!_currentPassenger)
                    return;
            }

            if (_currentPassenger.IsPassengerMoving()) return;

            Timer += dt;
            if (Timer < CheckDelay) return;
            Timer -= CheckDelay;

            GetCurrentPassengerTicketPrice();
        }

        internal override void EnqueuePassenger(PassengerManager p)
        {
            base.EnqueuePassenger(p);
            p.SetPassengerCarryAnimation(false);
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

                _currentPassenger = p;
                return;
            }
        }

        private void GetCurrentPassengerTicketPrice()
        {
            if (!_currentPassenger) return;
            uint ticketPrice = _currentPassenger.GetTicketPrice();
            EventBus.CorePassengerSignals.OnCheckedPassengerTicket?.Invoke(ticketPrice);
            CompleteCurrentPassenger();
        }
    }
}