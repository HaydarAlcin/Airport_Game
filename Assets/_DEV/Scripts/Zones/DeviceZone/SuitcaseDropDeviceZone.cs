using UnityEngine;
using System.Collections.Generic;
using DEV.Scripts.Player;
using DEV.Scripts.Signals;
using DG.Tweening;
using DEV.Scripts.SO.Zone.DeviceZone;

namespace DEV.Scripts.Zones.DeviceZone
{
    internal class SuitcaseDropDeviceZone : DeviceZone
    {
        [Space]
        [SerializeField] private Transform dropPoint;

        private readonly Queue<Transform> _droppedSuitcases = new();
        private PlayerStackController _playerStackController;

        private Ease _dropEase;

        private float _dropOffsetY;
        private float _dropJumpPower;
        private float _dropDuration;

        private uint _dropCount;

        protected override void SetSettings()
        {
            _playerStackController = EventBus.CorePlayerSignals.PlayerManager?.Invoke().GetPlayerStackController();

            var deviceZoneData = zoneData as SuitcaseDropZoneData_SO;
            _dropOffsetY = deviceZoneData.DropOffsetY;
            _dropJumpPower = deviceZoneData.DropJumpPower;
            _dropDuration = deviceZoneData.DropDuration;
            _dropEase = deviceZoneData.DropEase;
        }

        public override void Leave()
        {
            Active = false;
            SetCircleImageColor(zoneData.InactiveColor);
        }

        public override void Tick(float dt)
        {
            if (!Active || _playerStackController == null) return;

            Timer += dt;
            if (Timer < _playerStackController.TakeItemDelay) return;
            Timer -= _playerStackController.TakeItemDelay;

            var item = _playerStackController.TryRelease();
            if (item)
            {
                _droppedSuitcases.Enqueue(item);
                PlayDropAnimation(item);
            }
        }

        protected override void PlayDropAnimation(Transform item)
        {
            _dropCount++;

            if (!dropPoint) return;

            item.SetParent(dropPoint);

            Vector3 targetPos = GetTargetDropPos();

            item.DOJump(targetPos, _dropJumpPower, 1, _dropDuration)
                .SetEase(_dropEase)
                .Join(item.DORotateQuaternion(dropPoint.rotation, _dropDuration))
                .OnComplete(() =>
                {
                    item.position = targetPos;
                    item.localRotation = Quaternion.identity;
                });
        }

        protected override Vector3 GetTargetDropPos()
        {
            if (!dropPoint) return Vector3.zero;

            int index = Mathf.Max(0, (int)_dropCount - 1);

            return dropPoint.position + new Vector3(0f, _dropOffsetY * index, 0f);
        }

        private void SetCurrentSuitcaseYPositions()
        {
            int index = 0;
            foreach (var suitcase in _droppedSuitcases)
            {
                Vector3 targetPos = GetTargetDropPos();
                suitcase.position = dropPoint.position + new Vector3(0f, _dropOffsetY * index, 0f);
                index++;
            }
        }


        internal Transform TryRelease()
        {
            if (_droppedSuitcases.Count == 0)
                return null;

            var suitcase = _droppedSuitcases.Peek();
            return suitcase;
        }

        internal void RemoveDroppedSuitcase()
        {
            if (_droppedSuitcases.Count == 0)
                return;
            _droppedSuitcases.Dequeue();
            _dropCount = (uint)_droppedSuitcases.Count;
            SetCurrentSuitcaseYPositions();
        }
    }
}