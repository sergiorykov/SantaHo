using Ninject;
using Ninject.Modules;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Core.Configuration;
using SantaHo.Domain.IncomingLetters;
using SantaHo.Domain.Presents;
using SantaHo.Domain.SantaOffice;
using SantaHo.Infrastructure.Rabbit;
using SantaHo.Infrastructure.Rabbit.Queues;
using SantaHo.Infrastructure.Redis;

namespace SantaHo.Infrastructure.Modules
{
    public sealed class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRequireLoading>().To<RabbitConnectionFactory>().InSingletonScope();
            Bind<IRequireLoading>().To<RedisConnectionFactory>().InSingletonScope();

            IncomingLetters();
            ToyOrders();

            Bind<SettingsMigration>().ToSelf().InSingletonScope();
            Bind<ISettingsMigrationRegistrar>().ToMethod(x => x.Kernel.Get<SettingsMigration>()).InSingletonScope();

            Bind<ISettingsRepository>()
                .ToConstructor(x => new SettingsRepository(x.Inject<RedisConnectionFactory>(), new RedisKeyEvaluator()))
                .InSingletonScope();
        }

        private void ToyOrders()
        {
            Bind<ToyOrdersQueueManager>().ToSelf().InSingletonScope();

            Bind<IToyOrdersQueueManager>()
                .ToMethod(x => Kernel.Get<ToyOrdersQueueManager>())
                .InSingletonScope();

            Bind<IToyOrderCategoryRegistrar>()
                .ToMethod(x => Kernel.Get<ToyOrdersQueueManager>())
                .InSingletonScope();

            Bind<IToyOrdersEnqueuer>()
                .ToMethod(x => Kernel.Get<ToyOrdersQueueManager>().GetEnqueuer())
                .InSingletonScope();
        }

        private void IncomingLetters()
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