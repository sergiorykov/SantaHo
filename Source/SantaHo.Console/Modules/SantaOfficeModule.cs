using Ninject.Modules;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.SantaOffice.Service.Modules
{
    public sealed class SantaOfficeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Santa>().ToSelf().InSingletonScope();
        }
    }
}