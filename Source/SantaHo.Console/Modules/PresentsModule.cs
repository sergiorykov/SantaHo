using Ninject.Modules;
using SantaHo.Application.Presents;
using SantaHo.Domain.Presents;

namespace SantaHo.Console.Modules
{
    public sealed class PresentsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPresentOrderProcessor>().To<PresentOrderProcessor>().InSingletonScope();
        }
    }
}