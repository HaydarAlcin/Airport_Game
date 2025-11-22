using System;
using UnityEngine;
using DEV.Scripts.Enums;

namespace DEV.Scripts.Signals
{
    public class CorePassengerSignals
    {
        public Action<PassengerStatus, Transform> OnChangedPassengerState = delegate { };

        public Action<uint> OnCheckedPassengerTicket = delegate { };
    }
}