using Ninject;
using Ninject.Modules;
using SantaHo.Domain.Configuration;
using SantaHo.Domain.IncomingLetters;
using SantaHo.Infrastructure.Rabbit.Queues;
using SantaHo.Infrastructure.Redis;

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

            Bind<ISettingsRepository>().ToConstant(new SettingsRepository(RedisConnectionFactory.Create())).InSingletonScope();
        }
    }
}