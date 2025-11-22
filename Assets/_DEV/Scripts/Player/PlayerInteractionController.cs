using UnityEngine;
using DEV.Scripts.Interface;
using DEV.Scripts.Handler;

namespace DEV.Scripts.Player
{
    [RequireComponent(typeof(Collider))]
    internal class PlayerInteractionController : MonoBehaviour
    {
        private IInteractZone _zone;

        private string _interactZoneTag;

        internal void UpdateController() => _zone?.Tick(Time.deltaTime);

        internal void SetSettings() => _interactZoneTag = RuntimeStringParams.INTERACT_ZONE_TAG;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_interactZoneTag))
                InteractZone(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_interactZoneTag))
                ExitZone(other);
        }

        private void InteractZone(Collider other)
        {
            if (other.TryGetComponent<IInteractZone>(out var interactZone))
            {
                _zone = interactZone;
                _zone.Enter();
            }
        }

        private void ExitZone(Collider other)
        {
            if (other.TryGetComponent<IInteractZone>(out var interactZone))
            {
                if (ReferenceEquals(interactZone, _zone))
                {
                    _zone.Leave();
                    _zone = null;
                }
            }
        }
    }
}