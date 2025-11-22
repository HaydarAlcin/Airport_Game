namespace DEV.Scripts.Signals
{
    public static class EventBus
    {
        public static CoreGameSignals CoreGameSignals = new CoreGameSignals();

        public static CorePlayerSignals CorePlayerSignals = new CorePlayerSignals();
        public static CorePassengerSignals CorePassengerSignals = new CorePassengerSignals();
    }
}