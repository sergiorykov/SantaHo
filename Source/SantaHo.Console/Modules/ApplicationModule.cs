using Ninject.Modules;
using SantaHo.Application.IncomingLetters;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.Processing;
using SantaHo.Domain.Configuration;
using SantaHo.Domain.IncomingLetters;

namespace SantaHo.Console.Modules
{
    public sealed class ApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IIncomingLetterProcessor>().To<IncomingLetterProcessor>().InSingletonScope();
            Bind<ITaskFactory<IObservableMessage<Letter>, ProcessIncomingLetterSantaTask>>()
                .To<ProcessIncomingLetterTaskFactory>()
                .InSingletonScope();

            Bind<ISupportSettingsMigration>().To<IncomingLetterProcessor>().InSingletonScope();

            Bind<IApplicationService>()
                .To<IncomingLettersApplicationService>()
                .InSingletonScope();
        }
    }
}