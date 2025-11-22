using System;
using DEV.Scripts.Enums;

namespace DEV.Scripts.Signals
{
    public class CoreGameSignals
    {
        public Action<MoneyZoneUnlockType> OnMoneyZoneFinished;

        public Action<uint> OnMoneySpent;
        public Action<uint> OnMoneyEarned;
    }
}