namespace iBingo.Presentations.Services
{
    using System;

    public class SystemControlService : ISystemControlService
    {
        private readonly Action _shutdownAction;

        public SystemControlService(Action shutdownAction)
        {
            _shutdownAction = shutdownAction;
        }

        public void Shutdown()
        {
            _shutdownAction();
        }
    }
}