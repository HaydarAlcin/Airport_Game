using UnityEngine;
using UnityEngine.UI;

namespace DEV.Scripts.Zones
{
    internal class ZoneVisualController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer circleImage;

        internal void SetCircleImageColor(Color32 color) => circleImage.color = color;
    }
}
