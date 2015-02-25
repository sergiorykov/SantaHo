using Ninject.Modules;
using SantaHo.Core.ApplicationServices;
using SantaHo.FrontEnd.Service.Hosts;

namespace SantaHo.FrontEnd.Service.Bootstrap
{
    public class HostsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicationService>().To<HostApplicationService>().InSingletonScope();
        }
    }
}