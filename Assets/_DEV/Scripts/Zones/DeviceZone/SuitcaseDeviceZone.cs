using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using DEV.Scripts.SO.Zone.DeviceZone;

namespace DEV.Scripts.Zones.DeviceZone
{
    internal class SuitcaseDeviceZone : DeviceZone
    {
        [Space]
        [Header("Linked Zone")]
        [SerializeField] private SuitcaseDropDeviceZone linkedDropZone;

        [Space]
        [Header("Points")]
        [SerializeField] private Transform truckDropPoint;
        [SerializeField] private Transform firstJumpPoint;
        [SerializeField] private Transform suitcaseMovePoint;

        [Space]
        [Header("Truck Settings")]
        [SerializeField] private Transform truckTransform;
        [SerializeField] private Transform truckTargetTransform;

        [Space]
        [Header("Elevator Object")]
        [SerializeField] private Transform elevatorObject;

        private Vector3 _elevatorStartPos;
        private bool _elevatorStartPosCached;

        private readonly Queue<Transform> _droppedSuitcases = new();
       
        private Sequence _dropSequence;
        private Sequence _truckSequence;

        private float _moveDelay;
        private float _dropOffsetY;
        private float _dropJumpPower;
        private float _dropDuration;
        private float _moveDuration;
        private float _truckMoveDuration;
        private float _elevatorLiftHeight;
        private Ease _jumpEase;
        private Ease _moveEase;

        private uint _dropCount;

        protected override void SetSettings()
        {
            _dropSequence = DOTween.Sequence();
            _truckSequence = DOTween.Sequence();

            var deviceZoneData = zoneData as SuitcaseDeviceZoneData_SO;
            _dropOffsetY = deviceZoneData.DropOffsetY;
            _dropJumpPower = deviceZoneData.DropJumpPower;
            _dropDuration = deviceZoneData.DropDuration;
            _jumpEase = deviceZoneData.DropEase;
            _moveDelay = deviceZoneData.MoveDelay;
            _moveDuration = deviceZoneData.MoveDuration;
            _moveEase = deviceZoneData.MoveEase;
            _elevatorLiftHeight = deviceZoneData.ElevatorLiftHeight;

            _truckMoveDuration = deviceZoneData.TruckMoveDuration;

        }

        public override void Leave()
        {
            Active = false;
            SetCircleImageColor(zoneData.InactiveColor);
            Finish();
        }

        public override void Finish() => PlayTruckAnimation();

        public override void Tick(float dt)
        {
            if (!Active) return;

            Timer += dt;
            if (Timer < _moveDelay) return;
            Timer -= _moveDelay;

            var suitcase = linkedDropZone.TryRelease();
            if (suitcase)
                PlayDropAnimation(suitcase);
        }

        protected override void PlayDropAnimation(Transform suitcase)
        {
            if (!suitcase) return;
            if (_truckSequence.active || _dropSequence.active) return;

            linkedDropZone.RemoveDroppedSuitcase();

            _dropSequence?.Kill();
            _dropSequence = DOTween.Sequence();

            _dropSequence.Append(
                suitcase.DOMove(suitcaseMovePoint.position, _moveDuration)
                        .SetEase(_moveEase)
            );
            _dropSequence.Append(
                suitcase.DOJump(firstJumpPoint.position,
                                _dropJumpPower,
                                1,
                                _dropDuration)
                        .SetEase(_jumpEase)
            );
            _dropCount++;
            Vector3 finalPos = GetTargetDropPos();

            _dropSequence.Append(
                suitcase.DOJump(finalPos,
                                _dropJumpPower,
                                1,
                                _dropDuration)
                        .SetEase(_jumpEase)
            );
            if (!_elevatorStartPosCached)
            {
                _elevatorStartPos = elevatorObject.position;
                _elevatorStartPosCached = true;
            }

            float totalBeforeLastJump = _moveDuration + _dropDuration;
            float half = _dropDuration * 0.5f;

            Vector3 upPos = _elevatorStartPos + Vector3.up * _elevatorLiftHeight;

            _dropSequence.Insert(
                totalBeforeLastJump,
                elevatorObject.DOMove(upPos, half).SetEase(Ease.OutQuad)
            );
            _dropSequence.Insert(
                totalBeforeLastJump + half,
                elevatorObject.DOMove(_elevatorStartPos, half).SetEase(Ease.InQuad)
            );
            _dropSequence.OnComplete(() =>
            {
                AddSuitcase(suitcase, finalPos);
            });
        }

        protected override Vector3 GetTargetDropPos()
        {
            if (!truckDropPoint) return Vector3.zero;

            int index = Mathf.Max(0, (int)_dropCount - 1);

            return truckDropPoint.position + new Vector3(0f, _dropOffsetY * index, 0f);
        }

        private void PlayTruckAnimation()
        {
            if (_truckSequence.active || _dropSequence.active) return;

            Vector3 startPos = truckTransform.position;
            Quaternion startRot = truckTargetTransform.rotation;

            _truckSequence?.Kill();
            _truckSequence = DOTween.Sequence();

            _truckSequence.Append(
                truckTransform.DOMove(truckTargetTransform.position, _truckMoveDuration)
                         .SetEase(_moveEase)
            );

            _truckSequence.AppendCallback(() =>
            {
                while (_droppedSuitcases.Count > 0)
                {
                    var suitcase = _droppedSuitcases.Dequeue();
                    if (suitcase)
                        Destroy(suitcase.gameObject);
                }

                _dropCount = 0;
            });

            _truckSequence.Append(
                truckTransform.DOMove(startPos, _truckMoveDuration)
                         .SetEase(_moveEase)
            );
        }

        private void AddSuitcase(Transform item, Vector3 finalPos)
        {
            item.position = finalPos;
            if (truckDropPoint)
                item.SetParent(truckDropPoint, true);

            _droppedSuitcases.Enqueue(item);

            if (!Active && !_dropSequence.active && !_truckSequence.active)
                Finish();
        }
    }
}