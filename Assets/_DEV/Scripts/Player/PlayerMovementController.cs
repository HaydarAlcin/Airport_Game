using UnityEngine;
using DEV.Scripts.Signals;
using DEV.Scripts.SO.Player;
using DG.Tweening;

namespace DEV.Scripts.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private PlayerInputController playerInputController;
        [SerializeField] private Transform modelView;

        private Transform _cameraTransform;
        private Tween _autoMoveTween;

        private bool _canMove = true;
        private float _moveSpeed;
        private float _rotationSpeed;
        private float _inputDeadzone;
        private float _autoMoveDuration;

        public bool IsMoving { get; private set; }

        internal void UpdateController()
        {
            if (!_canMove) return;
            if (!TryGetMoveDirection(out var dir))
            {
                if (IsMoving)
                    EventBus.CorePlayerSignals.OnPlayerMovingValueChanged?.Invoke(false);

                IsMoving = false;
                return;
            }

            if (!IsMoving)
                EventBus.CorePlayerSignals.OnPlayerMovingValueChanged?.Invoke(true);

            IsMoving = true;
            ApplyMovement(dir);
            ApplyRotation(dir);
        }

        private bool TryGetMoveDirection(out Vector3 dir)
        {
            Vector2 move = ReadMoveInput();
            dir = ToWorldDirection(move);
            float mag = dir.magnitude;

            if (mag < _inputDeadzone) return false;

            dir /= mag;
            return true;
        }

        private void ApplyMovement(Vector3 dir)
        {
            controller.Move(dir * (_moveSpeed * Time.deltaTime));
        }

        private void ApplyRotation(Vector3 dir)
        {
            Quaternion target = Quaternion.LookRotation(dir, Vector3.up);
            modelView.rotation = Quaternion.RotateTowards(
                modelView.rotation, target, _rotationSpeed * Time.deltaTime);
        }

        private Vector2 ReadMoveInput() => playerInputController.Move();

        private Vector3 ToWorldDirection(Vector2 move)
        {
            Vector3 local = new Vector3(move.x, 0f, move.y);

            if (!_cameraTransform)
                return local;

            Vector3 f = _cameraTransform.forward; f.y = 0f; f.Normalize();
            Vector3 r = _cameraTransform.right; r.y = 0f; r.Normalize();
            return f * local.z + r * local.x;
        }

        internal void SetSettings(PlayerData_SO data)
        {
            _cameraTransform = Camera.main.transform;

            _inputDeadzone = data.InputDeadzone;
            _moveSpeed = data.MoveSpeed;
            _rotationSpeed = data.RotationSpeed;
            _autoMoveDuration = data.AutoMoveDuration;
        }

        internal void StartStairAutoMove(Vector3 position)
        {
            SetCanMove(false); 
            controller.enabled = false;
            _autoMoveTween?.Kill();

            EventBus.CorePlayerSignals.OnPlayerMovingValueChanged?.Invoke(false);

            _autoMoveTween = transform.parent.DOMove(position, _autoMoveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    SetCanMove(true);
                    _autoMoveTween = null;
                    controller.enabled = true;
                });
        }

        internal void SetCanMove(bool value) => _canMove = value;
    }
}