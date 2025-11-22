using DEV.Scripts.Enums;
using DG.Tweening;
using System;
using UnityEngine;

namespace DEV.Scripts.Passenger
{
    public class PassengerPathController : MonoBehaviour
    {
        private PassengerManager _manager;
        private PassengerPathStep[] _steps;

        private Action _onPathCompleted;

        private Vector3 _waitingPosition;

        private Sequence _seq;
        private Ease _moveEase;

        private float _rotateEaseSeconds;
        private float _walkSpeed;
        private float _runSpeed;

        private bool _isMoving = false;
        public bool IsMoving => _isMoving;

        internal void SetSettings(PassengerManager manager, PathSettings settings)
        {
            _manager = manager;
            _walkSpeed = settings.WalkSpeed;
            _runSpeed = settings.RunSpeed;
            _rotateEaseSeconds = settings.RotateEaseSeconds;

            _moveEase = settings.MoveEase;
        }

        internal void SetPath(PassengerPathStep[] steps, Vector3 targetPos, Action onPathCompleted = null)
        {
            _waitingPosition = targetPos;
            _steps = steps;
            _onPathCompleted = onPathCompleted;

            if (_isMoving)
                Cancel();

            _isMoving = true;
            FollowPath();
        }

        internal void SetTargetWaitingPos(Vector3 waitingPositionTarge)
        {
            _isMoving = true;
            _waitingPosition = waitingPositionTarge;
            MoveTargetWaitingPos();
        }

        private void FollowPath()
        {
            _seq?.Kill();

            if (_steps == null || _steps.Length == 0)
            {
                MoveTargetWaitingPos();
                return;
            }

            _seq = DOTween.Sequence();
            Vector3 prev = transform.position;

            for (int i = 0; i < _steps.Length; i++)
            {
                var s = _steps[i];
                if (!s.transform) continue;

                float speed = s.Locomotion == PassengerLocomotionType.Run ? _runSpeed : _walkSpeed;
                float dist = Vector3.Distance(prev, s.transform.position);
                float dur = speed > 0.001f ? dist / speed : 0f;

                _seq.AppendCallback(() =>
                {
                    bool isMoving = s.Locomotion == PassengerLocomotionType.Run ? true : false;
                    _manager?.SetPassengerMovingAnimation(isMoving);
                });

                _seq.Append(transform.DOLookAt(s.transform.position, _rotateEaseSeconds, AxisConstraint.Y));

                if (dur > 0f)
                    _seq.Append(transform.DOMove(s.transform.position, dur).SetEase(_moveEase));
                else
                    _seq.AppendInterval(0.01f);

                prev = s.transform.position;
            }

            _seq.OnComplete(() => MoveTargetWaitingPos());
        }

        private void MoveTargetWaitingPos()
        {
            float speed = _runSpeed;
            float dist = Vector3.Distance(transform.position, _waitingPosition);
            float dur = speed > 0.001f ? dist / speed : 0f;
            _seq = DOTween.Sequence();
            _seq.AppendCallback(() =>
            {
                _manager?.SetPassengerMovingAnimation(true);
            });
            _seq.Append(transform.DOLookAt(_waitingPosition, _rotateEaseSeconds, AxisConstraint.Y));
            if (dur > 0f)
                _seq.Append(transform.DOMove(_waitingPosition, dur).SetEase(_moveEase));
            else
                _seq.AppendInterval(0.01f);
            _seq.AppendCallback(() =>
            {
                _manager?.SetPassengerMovingAnimation(false);
                _isMoving = false;
                _onPathCompleted?.Invoke();
            });
        }

        private void Cancel()
        {
            _seq?.Kill();
        }

        void OnDisable() => _seq?.Kill();
    }

    internal struct PathSettings
    {
        public float WalkSpeed;
        public float RunSpeed;
        public float RotateEaseSeconds;
        public Ease MoveEase;
    }
}