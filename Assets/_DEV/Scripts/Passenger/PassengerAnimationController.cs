using UnityEngine;
using DEV.Scripts.Handler;

namespace DEV.Scripts.Passenger
{
    public class PassengerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator passengerAnimator;

        private int _isMovingHash = Animator.StringToHash(RuntimeStringParams.IS_MOVING_ANIMATION);
        private int _isCarryingHash = Animator.StringToHash(RuntimeStringParams.IS_CARRYING_ANIMATION);

        internal void SetPassengerCarryAnimation(bool isCarrying) => passengerAnimator?.SetBool(_isCarryingHash, isCarrying);

        internal void SetPassengerMovingAnimation(bool isMoving) => passengerAnimator?.SetBool(_isMovingHash, isMoving);
    }
}