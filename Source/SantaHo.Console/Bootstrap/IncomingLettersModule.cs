using System;
using Ninject;
using Ninject.Modules;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.SantaOffice.Service.IncomingLetters;

namespace SantaHo.SantaOffice.Service.Bootstrap
{
    public sealed class IncomingLettersModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IncomingLetterQueue>().ToSelf().InSingletonScope();
            Bind<IApplicationResource>().ToMethod(x => x.Kernel.Get<IncomingLetterQueue>());

            Bind<IApplicationService>().To<IncomingLetterApplicationService>().InSingletonScope();
        }
    }
}
