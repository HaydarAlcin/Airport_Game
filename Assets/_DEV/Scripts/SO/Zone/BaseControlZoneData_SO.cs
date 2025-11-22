using UnityEngine;

namespace DEV.Scripts.SO.Zone
{
    internal class BaseControlZoneData_SO : ScriptableObject
    {
        [Header("Circle Image")]
        [SerializeField] private Color32 activeColor;
        [SerializeField] private Color32 inactiveColor;

        public Color32 ActiveColor => activeColor;
        public Color32 InactiveColor => inactiveColor;
    }
}