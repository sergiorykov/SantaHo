using Ninject;
using Ninject.Modules;
using SantaHo.Core.ApplicationServices;
using SantaHo.Domain.IncomingLetters;
using SantaHo.Infrastructure.Queues;

namespace SantaHo.Infrastructure.Modules
{
    public sealed class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IncomingLettersQueueManager>().ToSelf().InSingletonScope();

            Bind<IIncomingLettersEnqueuer>()
                .ToMethod(x => Kernel.Get<IncomingLettersQueueManager>().GetEnqueuer())
                .InSingletonScope();

            Bind<IIncomingLettersDequeuer>()
                .ToMethod(x => Kernel.Get<IncomingLettersQueueManager>().GetDequeuer())
                .InSingletonScope();
        }
    }
}