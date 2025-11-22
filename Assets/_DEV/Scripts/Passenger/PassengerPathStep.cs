using DEV.Scripts.Enums;
using UnityEngine;

namespace DEV.Scripts.Passenger
{
    internal class PassengerPathStep : MonoBehaviour
    {
        [SerializeField] private PassengerLocomotionType locomotion;

        public PassengerLocomotionType Locomotion => locomotion;
    }
}