using DEV.Scripts.Player;
using System;

namespace DEV.Scripts.Signals
{
    public class CorePlayerSignals
    {
        public Func<PlayerManager> PlayerManager = delegate { return null; };

        public Action<bool> OnPlayerMovingValueChanged = delegate { };
        public Action<bool> OnPlayerCarryingValueChanged = delegate { };
    }
}