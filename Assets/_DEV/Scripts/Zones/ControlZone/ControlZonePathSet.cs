using DEV.Scripts.Passenger;
using UnityEngine;

namespace DEV.Scripts.Zones.ControlZone
{
    internal class ControlZonePathSet : MonoBehaviour
    {
        [SerializeField] private PassengerPathStep[] steps;
        [SerializeField] private Transform firstWaitingTransform;

        public PassengerPathStep[] Steps => steps;
        public Transform FirstWaitingPoint => firstWaitingTransform;
    }
}