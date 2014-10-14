using Ninject.Modules;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.Console.Modules
{
    public sealed class SantaOfficeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Santa>().ToSelf().InSingletonScope();
        }
    }
}