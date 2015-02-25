using System;
using Ninject.Modules;
using SantaHo.SantaOffice.Service.Presents;

namespace SantaHo.SantaOffice.Service.Bootstrap
{
    public sealed class PresentsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPresentOrderProcessor>().To<PresentOrderProcessor>().InSingletonScope();
        }
    }
}
