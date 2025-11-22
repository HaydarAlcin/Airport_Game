using DG.Tweening;
using UnityEngine;

namespace DEV.Scripts.SO.Zone.DeviceZone
{
    [CreateAssetMenu(fileName = "SuitcaseDeviceZoneData_SO", menuName = "ScriptableObjects/Zone/DeviceZone/SuitcaseDeviceZoneData_SO")]
    internal class SuitcaseDeviceZoneData_SO : BaseControlZoneData_SO
    {
        [Space]
        [Header("Drop Settings")]
        [SerializeField] private float dropOffsetY = 1f;
        [SerializeField] private float dropJumpPower = 2f;
        [SerializeField] private float dropDuration = 0.4f;
        [SerializeField] private Ease dropEase = Ease.OutCubic;

        [Space]
        [Header("Move Settings")]
        [SerializeField] private float moveDelay = 0.2f;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private Ease moveEase = Ease.Linear;

        [Space]
        [Header("Truck Settings")]
        [SerializeField, Min(0f)] private float truckMoveDuration = 2f;

        [Space]
        [Header("Elevator Settings")]
        [SerializeField] private float elevatorLiftHeight = 3f;

        public float DropOffsetY => dropOffsetY;
        public float DropJumpPower => dropJumpPower;
        public float DropDuration => dropDuration;
        public Ease DropEase => dropEase;

        public float MoveDelay => moveDelay;
        public float MoveDuration => moveDuration;
        public Ease MoveEase => moveEase;

        public float TruckMoveDuration => truckMoveDuration;

        public float ElevatorLiftHeight => elevatorLiftHeight;
    }
}