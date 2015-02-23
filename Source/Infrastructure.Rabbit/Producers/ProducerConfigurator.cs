using System;
using FluffyRabbit.Exchanges;
using FluffyRabbit.Queues;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using RabbitMQ.Client;

namespace FluffyRabbit.Producers
{
    public sealed class ProducerConfigurator
    {
        private Option<RabbitExchangeConfiguration> _exchange = Option<RabbitExchangeConfiguration>.Empty;
        private Option<RabbitQueueConfiguration> _queue = Option<RabbitQueueConfiguration>.Empty;

        public ProducerConfigurator UseExchange(Action<IRabbitExchangeConfiguration> configure)
        {
            var exchange = new RabbitExchangeConfiguration();
            configure(exchange);

            _exchange = exchange.ToOption();
            return this;
        }

        public ProducerConfigurator Queue(Action<IRabbitQueueConfiguration> configure)
        {
            var queue = new RabbitQueueConfiguration();
            configure(queue);

            _queue = queue.ToOption();
            return this;
        }

        public IMessageEnqueuer<TMessage> Create<TMessage>(IConnection connection)
        {
            _queue.ThrowOnEmpty(() => new InvalidOperationException("Queue"));
            var channel = connection.CreateModel();

            try
            {
                _exchange.Do(x => x.Apply(channel));
                return _queue
                    .Map(x => x.CreateEnqueuer<TMessage>(channel, _exchange))
                    .Value;
            }
            catch (Exception e)
            {
                channel.Close();
                throw new InvalidOperationException();
            }
        }
    }
}