
using UnityEngine;

namespace DEV.Scripts.Zones.DeviceZone
{
    internal class DeviceZone : BaseZone
    {
        protected virtual void PlayDropAnimation(Transform item) { }

        protected virtual Vector3 GetTargetDropPos() { return Vector3.zero; }
    }
}
