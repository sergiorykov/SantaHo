using Ninject.Modules;
using SantaHo.Core.ApplicationServices;

namespace SantaHo.FrontEnd.Service.Hosts
{
    public class HostModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicationService>().To<HostApplicationService>().InSingletonScope();
        }
    }
}