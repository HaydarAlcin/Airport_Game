using UnityEngine;
using DEV.Scripts.SO.Player;
using DEV.Scripts.Signals;

namespace DEV.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Player Data")]
        [SerializeField] private PlayerData_SO playerData;

        [Header("Player Controllers")]
        [SerializeField] private PlayerStackController playerStackController;
        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private PlayerInteractionController playerInteractionController;

        // Script Execution Order
        private void Awake() => InitializePlayer();

        private void OnDisable() => EventBus.CorePlayerSignals.PlayerManager = null;

        private void InitializePlayer()
        {
            EventBus.CorePlayerSignals.PlayerManager = () => this;
            playerMovementController?.SetSettings(playerData);
            playerStackController?.SetSettings(playerData);
            playerInteractionController?.SetSettings();
        }

        private void Update()
        {
            playerMovementController?.UpdateController();
            playerInteractionController?.UpdateController();
        }

        internal PlayerStackController GetPlayerStackController() => playerStackController;

        internal void SetCanMove(bool value) => playerMovementController?.SetCanMove(value);

        internal void StartStairAutoMove(Vector3 position) => playerMovementController.StartStairAutoMove(position);
    }
}