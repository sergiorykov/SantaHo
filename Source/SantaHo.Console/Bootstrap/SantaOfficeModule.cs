using System;
using Ninject.Modules;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.SantaOffice.Service.Bootstrap
{
    public sealed class SantaOfficeModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Santa>().ToSelf().InSingletonScope();
        }
    }
}
