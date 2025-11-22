using System;
using UnityEngine;
using DEV.Scripts.Enums;
using DEV.Scripts.Signals;

namespace DEV.Scripts.GameCamera
{
    public class GameCameraController : MonoBehaviour
    {
        [SerializeField] private GameObject playerVirtualCam;
        [SerializeField] private GameObject paintBoardVirtualCam;

        private void OnEnable() => EventBus.CoreGameSignals.OnMoneyZoneFinished += OnMoneyZoneFinished;

        private void OnDisable() => EventBus.CoreGameSignals.OnMoneyZoneFinished -= OnMoneyZoneFinished;

        private void OnMoneyZoneFinished(MoneyZoneUnlockType type)
        {
            if (!(type == MoneyZoneUnlockType.OpenPainting)) return;

            playerVirtualCam?.SetActive(false);
            paintBoardVirtualCam?.SetActive(true);
        }
    }
}