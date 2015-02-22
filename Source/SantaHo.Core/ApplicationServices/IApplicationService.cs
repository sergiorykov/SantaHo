namespace SantaHo.Core.ApplicationServices
{
    public interface IConfigurable
    {
        void Configure(IStartupSettings startupSettings);
    }

    public interface IApplicationService
    {
        void Start(IStartupSettings startupSettings);
        void Stop();
    }
}