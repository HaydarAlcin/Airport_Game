using UnityEngine;
using DEV.Scripts.Enums;

namespace DEV.Scripts.SO.Zone.MoneyZone
{
    [CreateAssetMenu(fileName = "MoneyZoneData_SO", menuName = "ScriptableObjects/MoneyZone/MoneyZoneData_SO")]
    public class MoneyZoneData_SO : ScriptableObject
    {
        [Min(1)] public uint TotalCost = 50;
        [Min(0)] public uint DrainPerSec = 2;
        [Min(1)] public uint MinChunk = 1;

        [Header("Finish Event")]
        public MoneyZoneUnlockType UnlockType = MoneyZoneUnlockType.None;
    }
}