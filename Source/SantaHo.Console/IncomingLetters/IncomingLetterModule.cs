using Ninject.Modules;
using SantaHo.Core.ApplicationServices;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public sealed class IncomingLetterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicationService>().To<IncomingLetterApplicationService>().InSingletonScope();
        }
    }
}