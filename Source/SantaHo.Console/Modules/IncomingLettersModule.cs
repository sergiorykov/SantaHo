using Ninject.Modules;
using SantaHo.Application.IncomingLetters;
using SantaHo.Core.ApplicationServices;
using SantaHo.Domain.Configuration;
using SantaHo.Domain.IncomingLetters;

namespace SantaHo.Console.Modules
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