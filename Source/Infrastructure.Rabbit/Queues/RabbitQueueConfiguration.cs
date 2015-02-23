using FluffyRabbit.Exchanges;
using FluffyRabbit.Producers;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using RabbitMQ.Client;

namespace FluffyRabbit.Queues
{
    public sealed class RabbitQueueConfiguration : IRabbitQueueConfiguration
    {
        public bool Durable { get; set; }
        public bool AutoDelete { get; set; }
        public ushort PrefetchCount { get; set; }
        public string RoutingKey { get; set; }
        public string Name { get; set; }
        
        public IMessageEnqueuer<TMessage> CreateEnqueuer<TMessage>(IModel channel)
        {
            return CreateEnqueuer<TMessage>(channel, Option<RabbitExchangeConfiguration>.Empty);
        }

        public IMessageEnqueuer<TMessage> CreateEnqueuer<TMessage>(IModel channel, Option<RabbitExchangeConfiguration> exchange)
        {
            string exchangeName = exchange
                .Map(x => x.Name)
                .MapOnEmpty(null)
                .Value;

            channel.QueueDeclare(Name, Durable, false, AutoDelete, null);
            channel.QueueBind(Name, exchangeName, RoutingKey);

            if (PrefetchCount > 0)
            {
                channel.BasicQos(0, PrefetchCount, false);
            }

            return new RabbitEnqueuer<TMessage>(channel, exchangeName, RoutingKey);
        }
    }
}