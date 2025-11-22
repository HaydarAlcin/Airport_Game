namespace DEV.Scripts.Interface
{
    internal interface IInteractZone
    {
        public void Enter();
        public void Tick(float dt);
        public void Leave();
        public void Finish();
    }
}