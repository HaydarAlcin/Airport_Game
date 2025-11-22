using UnityEngine;
using DG.Tweening;
using DEV.Scripts.SO.Player;
using DEV.Scripts.Signals;
using System.Collections.Generic;

namespace DEV.Scripts.Player
{
    public class PlayerStackController : MonoBehaviour
    {
        [SerializeField] private Transform stackParent;

        private Stack<Transform> _itemStack = new Stack<Transform>();

        private bool _isCarrying;

        private Tween _tween;
        private Ease _takeItemEase;
        private Ease _dropEase;

        private uint _stackCount;
        private uint _maxStackCount;

        private float _collectDuration;
        private float _stackOffsetY;
        private float _takeItemDelay;

        private float _dropOffsetY;
        private float _dropJumpPower;
        private float _dropDuration;

        public float TakeItemDelay => _takeItemDelay;

        private void OnDisable() => _tween?.Kill();

        internal void SetSettings(PlayerData_SO data)
        {
            _itemStack.Clear();

            _maxStackCount = data.MaxStackCapacity;
            _stackOffsetY = data.StackOffsetY;
            _takeItemDelay = data.TakeItemDelay;
            _takeItemEase = data.TakeItemEase;
            _collectDuration = data.CollectDuration;

            _dropDuration = data.DropDuration;
            _dropOffsetY = data.DropOffsetY;
            _dropEase = data.DropEase;
            _dropJumpPower = data.DropJumpPower;
            _stackCount = 0;

        }

        internal bool TryCollect(Transform item)
        {
            if (_stackCount >= _maxStackCount)
                return false;

            if(!_isCarrying) EventBus.CorePlayerSignals.OnPlayerCarryingValueChanged?.Invoke(true);
            _isCarrying = true;
            _stackCount++;

            PlayItemAnimation(item);
            return true;
        }

        internal Transform TryRelease()
        {
            if (_itemStack.Count == 0) return null;

            Transform item = _itemStack.Pop();
            if (!item) return null;

            if (_itemStack.Count == 0)
            {
                _isCarrying = false;
                EventBus.CorePlayerSignals.OnPlayerCarryingValueChanged?.Invoke(false);
            }

            return item;
        }

        private void PlayItemAnimation(Transform item)
        {
            Vector3 targetLocalPos = new Vector3(0f, _stackOffsetY * _stackCount, 0f);

            item.SetParent(stackParent);
            _itemStack.Push(item);

            _tween.Complete();
            _tween = item.DOLocalJump(targetLocalPos, 1f, 1, _collectDuration)
                .SetEase(_takeItemEase)
                .SetDelay(_takeItemDelay)
                .Join(item.DOLocalRotateQuaternion(Quaternion.identity, _collectDuration))
                .OnComplete(() =>
                {
                    item.localPosition = targetLocalPos;
                    item.localRotation = Quaternion.identity;
                });
        }
    }
}