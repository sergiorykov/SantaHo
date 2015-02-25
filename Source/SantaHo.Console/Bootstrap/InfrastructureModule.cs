using System;
using Ninject;
using Ninject.Modules;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Core.Configuration;
using SantaHo.SantaOffice.Service.Infrastructure.Rabbit;
using SantaHo.SantaOffice.Service.Infrastructure.Rabbit.Queues;
using SantaHo.SantaOffice.Service.Infrastructure.Redis;
using SantaHo.SantaOffice.Service.Toys;

namespace SantaHo.SantaOffice.Service.Bootstrap
{
    public sealed class InfrastructureModule : NinjectModule
    {
        public override void Load()
        {
            Bind<RabbitConnectionFactory1>().ToSelf().InSingletonScope();
            Bind<IRequireLoading>().ToMethod(c => c.Kernel.Get<RabbitConnectionFactory1>());

            Bind<RedisConnectionFactory>().ToSelf().InSingletonScope();
            Bind<IRequireLoading>().ToMethod(c => c.Kernel.Get<RedisConnectionFactory>());

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
    }
}
