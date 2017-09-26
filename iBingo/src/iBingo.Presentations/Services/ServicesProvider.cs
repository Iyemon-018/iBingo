namespace iBingo.Presentations.Services
{
    public class ServicesProvider : IServicesProvider
    {
        public ServicesProvider(ISystemControlService systemControlService)
        {
            SystemControlService = systemControlService;
        }

        public ISystemControlService SystemControlService { get; }
    }
}