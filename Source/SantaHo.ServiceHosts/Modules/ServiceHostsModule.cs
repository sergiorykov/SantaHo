using Ninject.Modules;
using SantaHo.Core.ApplicationServices;
using SantaHo.ServiceHosts.Processors;

namespace SantaHo.ServiceHosts.Modules
{
    public class ServiceHostsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<WishListProcessor>().ToSelf();
            Bind<MonitoringProcessor>().ToSelf();

            Bind<IApplicationService>().To<SantaPostOfficeApplicationService>();
        }
    }
}