using UnityEngine;
using DEV.Scripts.Handler;
using DEV.Scripts.Signals;

namespace AirportGame.Player
{
    internal class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;

        private int _isMovingHash = Animator.StringToHash(RuntimeStringParams.IS_MOVING_ANIMATION);
        private int _isCarryingHash = Animator.StringToHash(RuntimeStringParams.IS_CARRYING_ANIMATION);

        private void OnEnable()
        {
            EventBus.CorePlayerSignals.OnPlayerMovingValueChanged += OnPlayerMovingValueChanged;
            EventBus.CorePlayerSignals.OnPlayerCarryingValueChanged += OnPlayerCarryingValueChanged;
        }

        private void OnDisable()
        {
            EventBus.CorePlayerSignals.OnPlayerMovingValueChanged -= OnPlayerMovingValueChanged;
            EventBus.CorePlayerSignals.OnPlayerCarryingValueChanged -= OnPlayerCarryingValueChanged;
        }

        private void OnPlayerMovingValueChanged(bool isMoving) => playerAnimator?.SetBool(_isMovingHash, isMoving);

        private void OnPlayerCarryingValueChanged(bool isCarrying) => playerAnimator?.SetBool(_isCarryingHash, isCarrying);
    }
}