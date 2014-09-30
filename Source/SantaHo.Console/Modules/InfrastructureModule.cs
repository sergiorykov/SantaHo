using Ninject.Modules;
using SantaHo.Core.ApplicationServices;
using SantaHo.Domain.IncomingLetters;
using SantaHo.Infrastructure.Queues;
using SantaHo.Infrastructure.Services;

namespace SantaHo.Console.Modules
{
    public sealed class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IIncomingLettersQueue>().To<IncomingLettersQueue>();
            Bind<IApplicationService>().To<IncomingLetterQueueProcessingService>();
        }
    }
}