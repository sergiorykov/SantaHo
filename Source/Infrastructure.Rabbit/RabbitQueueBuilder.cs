using RabbitMQ.Client;

namespace SantaHo.Infrastructure.Rabbit
{
    public sealed class RabbitQueueBuilder
    {
        private bool _autoDelete;
        private bool _durable;
        private string _queueName;
        private string _routingKey;
        private readonly IModel _channel;
        private readonly string _exchangeName;

        public RabbitQueueBuilder(IModel channel, string exchangeName)
        {
            _channel = channel;
            _exchangeName = exchangeName;
        }

        public RabbitQueueBuilder Durable()
        {
            _durable = true;
            return this;
        }

        public RabbitQueueBuilder AutoDelete()
        {
            _autoDelete = true;
            return this;
        }

        public RabbitQueueBuilder PrefetchMax(ushort prefetchCount)
        {
            _channel.BasicQos(0, prefetchCount, false);
            return this;
        }

        public RabbitQueueBuilder USeRoutingKey(string routingKey)
        {
            _routingKey = routingKey;
            return this;
        }

        public RabbitQueueBuilder ForQueue(string queueName)
        {
            _queueName = queueName;
            return this;
        }

        public IMessageEnqueuer<TMessage> CreateEnqueuer<TMessage>()
        {
            _channel.QueueDeclare(_queueName, _durable, false, _autoDelete, null);
            _channel.QueueBind(_queueName, _exchangeName, _routingKey);
            return new RabbitEnqueuer<TMessage>(_channel, _exchangeName, _routingKey);
        }
    }
}