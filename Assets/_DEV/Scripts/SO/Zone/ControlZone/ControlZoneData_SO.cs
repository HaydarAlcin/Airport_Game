using DEV.Scripts.Enums;
using UnityEngine;


namespace DEV.Scripts.SO.Zone.ControlZone
{
    [CreateAssetMenu(fileName = "ControlZoneData_SO", menuName = "ScriptableObjects/Zone/ControlZone/Control Zone Data")]
    internal class ControlZoneData_SO : BaseControlZoneData_SO
    {
        [Space(20)]
        [Header("States")]
        [SerializeField] private PassengerStatus currentPassengerState;
        [SerializeField] private PassengerStatus nextPassengerState;
        [Space]
        [SerializeField] private float checkDelay = 1f;
        [SerializeField, Min(0f)] private float waitingSpacing = 2f;

        public PassengerStatus CurrentPassengerState => currentPassengerState;
        public PassengerStatus NextPassengerState => nextPassengerState;
        public float WaitingSpacing => waitingSpacing;
        public float CheckDelay => checkDelay;
    }
}