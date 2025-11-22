using UnityEngine;
using DG.Tweening;

namespace DEV.Scripts.SO.Player
{
    [CreateAssetMenu(fileName = "PlayerData_SO", menuName = "ScriptableObjects/Player/PlayerData_SO")]
    internal class PlayerData_SO : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float autoMoveDuration = 2f;
        [SerializeField, Min(0f)] private float moveSpeed = 4f;
        [SerializeField, Min(0f)] private float rotationSpeed = 540f;
        [SerializeField, Range(0f, 0.2f)] private float deadzone = 0.05f;

        [Space]
        [Header("Stacking")]
        [SerializeField] private Ease takeItemEase = Ease.OutBack;
        [SerializeField] private Ease dropEase = Ease.OutQuad;
        [SerializeField] private float collectDuration = 0.5f;
        [SerializeField] private float dropDuration = 0.5f;
        [SerializeField] private uint maxStackCapacity = 10;
        [SerializeField] private float stackOffsetY = 0.25f;
        [SerializeField] private float dropOffsetY = 0.25f;
        [SerializeField] private float takeItemDelay = 0.3f;

        [SerializeField] private float dropJumpPower = 1f;

        public float MoveSpeed => moveSpeed;
        public float RotationSpeed => rotationSpeed;
        public float InputDeadzone => deadzone;

        public Ease TakeItemEase => takeItemEase;
        public Ease DropEase => dropEase;
        public float CollectDuration => collectDuration;
        public float DropDuration => dropDuration;
        public uint MaxStackCapacity => maxStackCapacity;
        public float StackOffsetY => stackOffsetY;
        public float DropOffsetY => dropOffsetY;
        public float TakeItemDelay => takeItemDelay;
        public float DropJumpPower => dropJumpPower;

        public float AutoMoveDuration => autoMoveDuration;
    }
}