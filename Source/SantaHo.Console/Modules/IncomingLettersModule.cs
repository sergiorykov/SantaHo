using Ninject.Modules;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.Configuration;
using SantaHo.Domain.IncomingLetters;
using SantaHo.SantaOffice.Service.IncomingLetters;

namespace SantaHo.SantaOffice.Service.Modules
{
    public sealed class IncomingLettersModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IIncomingLetterProcessor>().To<IncomingLetterProcessor>().InSingletonScope();
            Bind<ProcessIncomingLetterTaskFactory>().ToSelf().InSingletonScope();
            Bind<ISupportSettingsMigration>().To<IncomingLetterProcessor>().InSingletonScope();

            Bind<IApplicationService>()
                .To<IncomingLettersApplicationService>()
                .InSingletonScope();
        }
    }
}