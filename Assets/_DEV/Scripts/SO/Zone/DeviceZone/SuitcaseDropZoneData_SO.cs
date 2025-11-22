using DG.Tweening;
using UnityEngine;

namespace DEV.Scripts.SO.Zone.DeviceZone
{
    [CreateAssetMenu(fileName = "SuitcaseDropZoneData_SO", menuName = "ScriptableObjects/Zone/DeviceZone/SuitcaseDropZoneData_SO")]
    internal class SuitcaseDropZoneData_SO : BaseControlZoneData_SO
    {
        [Space]
        [Header("Drop Settings")]
        [SerializeField] private float dropOffsetY = 1f;
        [SerializeField] private float dropJumpPower = 2f;
        [SerializeField] private float dropDuration = 0.4f;
        [SerializeField] private Ease dropEase = Ease.OutCubic;

        public float DropOffsetY => dropOffsetY;
        public float DropJumpPower => dropJumpPower;
        public float DropDuration => dropDuration;
        public Ease DropEase => dropEase;
    }
}