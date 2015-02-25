using System;
using FluffyRabbit.Queues;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using RabbitMQ.Client;

namespace FluffyRabbit.Consumers
{
    public sealed class ConsumerConfigurator
    {
        private Option<RabbitQueueConfiguration> _queue = Option<RabbitQueueConfiguration>.Empty;

        public ConsumerConfigurator Queue(Action<IRabbitQueueConfiguration> configure)
        {
            var queue = new RabbitQueueConfiguration();
            configure(queue);

            _queue = queue.ToOption();
            return this;
        }

        public IObservableMessageDequeuer<TMessage> Create<TMessage>(IConnection connection)
        {
            _queue.ThrowOnEmpty(() => new InvalidOperationException("Queue"));
            var channel = connection.CreateModel();

            try
            {
                return _queue
                    .Map(x => x.CreateDequeuer<TMessage>(channel))
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
