using DEV.Scripts.Player;
using UnityEngine;

namespace DEV.Scripts.Stairs
{
    internal class StairInteractionController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform targetPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if (!targetPoint) return;

            var manager = other.GetComponent<PlayerManager>();

            manager.StartStairAutoMove(targetPoint.position);
        }
    }
}