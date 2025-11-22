using TMPro;
using UnityEngine;
using DEV.Scripts.Passenger;
using DEV.Scripts.SO.Zone.ControlZone;

namespace DEV.Scripts.Zones.ControlZone
{
    internal class AirplaneExitControlZone : ControlZone
    {
        [SerializeField] private TextMeshPro passengerCountText;

        private uint _totalPassengerCount;
        private uint _passengerCountInAirPlane;

        protected override void SetSettings()
        {
            var airplaneExitData = zoneData as ControlZoneData_SO;
            _currentPassengerState = airplaneExitData.CurrentPassengerState;
            _nextPassengerState = airplaneExitData.NextPassengerState;

            _totalPassengerCount = Passengers.Instance.PassengerCount;
            _passengerCountInAirPlane = 0;
            string passengerCountStr = $"{_passengerCountInAirPlane}/{_totalPassengerCount}";
            passengerCountText?.SetText(passengerCountStr);
        }

        internal override void OnPassengerPathCompleted()
        {
            _passengerCountInAirPlane++;
            var p = _passengers.Peek();
            p?.gameObject.SetActive(false);

            if (_passengers.Count > 0)
                _passengers.Dequeue();

            string passengerCountStr = $"{_passengerCountInAirPlane}/{_totalPassengerCount}";
            passengerCountText?.SetText(passengerCountStr);
        }

        internal override void EnqueuePassenger(PassengerManager p) => base.EnqueuePassenger(p);
    }
}