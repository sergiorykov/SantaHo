using System;
using RabbitMQ.Client;

namespace FluffyRabbit.Consumers
{
    public class ObservableMessageDequeuer<TMessage> : IObservableMessageDequeuer<TMessage>
    {
        private readonly IModel _channel;
        private readonly QueueingBasicConsumer _consumer;

        public ObservableMessageDequeuer(IModel channel, string queueName)
        {
            _channel = channel;
            _consumer = new QueueingBasicConsumer(_channel);
            _channel.BasicConsume(queueName, false, _consumer);
        }

        public IObservableMessage<TMessage> Dequeue()
        {
            var deliverEventArgs = _consumer.Queue.Dequeue();
            return new ObservableMessage<TMessage>(deliverEventArgs, _channel);
        }

        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Dispose();
            }
        }
    }
}
