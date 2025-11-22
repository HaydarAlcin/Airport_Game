using UnityEngine;
using System.Collections.Generic;
using DEV.Scripts.Signals;
using DEV.Scripts.Enums;
using DEV.Scripts.SO.Passenger;
using System;

namespace DEV.Scripts.Passenger
{
    public class PassengerManager : MonoBehaviour
    {
        [SerializeField] private PassengerAnimationController animationController;

        private PassengerPathController _pathController;
        private PassengerStackController _stackController;

        private uint _ticketPrice;

        private void Awake()
        {
            _pathController = GetComponent<PassengerPathController>();
            _stackController = GetComponent<PassengerStackController>();
        }

        internal void SetSettings(PassengerConfig_SO configData)
        {
            uint suitcaseCount = configData.GetRandomSuitcaseCount();
            _ticketPrice = suitcaseCount * configData.PricePerSuitcase;
            _stackController.SetManager(this);
            _stackController.SetSettings(configData.SuitcasePrefab, suitcaseCount, configData.SuitcaseOffsetY);

            var pathSettings = new PathSettings()
            {
                MoveEase = configData.MoveEase,
                RotateEaseSeconds = configData.RotateEaseSeconds,
                WalkSpeed = configData.WalkSpeed,
                RunSpeed = configData.RunSpeed
            };
            _pathController.SetSettings(this, pathSettings);
        }

        internal List<Transform> GetCollectableSuitcases() => _stackController.GetAllSuitcases();

        internal void OnSuitcaseCollected(Transform item) => _stackController.RemoveSuitcase(item);

        internal void SwitchState(PassengerStatus state) => EventBus.CorePassengerSignals.OnChangedPassengerState?.Invoke(state, this.transform);

        internal void SetPassengerCarryAnimation(bool isCarrying) => animationController.SetPassengerCarryAnimation(isCarrying);

        internal void SetPassengerMovingAnimation(bool isMoving) => animationController.SetPassengerMovingAnimation(isMoving);

        internal void SetPath(PassengerPathStep[] steps, Vector3 target, Action onPathCompleted = null) => _pathController.SetPath(steps, target, onPathCompleted);

        internal void SetTargetWaitingPos(Vector3 waitingPositionTarge) => _pathController.SetTargetWaitingPos(waitingPositionTarge);

        internal bool IsPassengerMoving() => _pathController.IsMoving;

        internal uint GetTicketPrice() => _ticketPrice;
    }
}