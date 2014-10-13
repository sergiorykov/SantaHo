using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SantaHo.Core.Processing;
using SantaHo.Domain.IncomingLetters;
using ServiceStack.Text;

namespace SantaHo.Infrastructure.Rabbit.Queues
{
    public sealed class IncomingLettersQueueManager
    {
        private const string ExchangeName = "incoming-letters-direct-exchange";
        private const string QueueName = "incoming-letters";
        private const string RoutingKey = "letter";
        private readonly IConnection _connection;

        public IncomingLettersQueueManager(IConnection connection)
        {
            _connection = connection;
        }

        public IIncomingLettersEnqueuer GetEnqueuer()
        {
            return new IncomingLettersEnqueuer(_connection);
        }

        public IIncomingLettersDequeuer GetDequeuer()
        {
            return new IncomingLettersDequeuer(_connection);
        }

        private sealed class IncomingLettersDequeuer : IIncomingLettersDequeuer
        {
            private readonly IModel _channel;
            private readonly QueueingBasicConsumer _consumer;

            public IncomingLettersDequeuer(IConnection connection)
            {
                _channel = connection.CreateModel();
                _channel.QueueDeclare(QueueName, true, false, false, null);
                _channel.BasicQos(0, 16*1024, false);
                _consumer = new QueueingBasicConsumer(_channel);
                _channel.BasicConsume(QueueName, false, _consumer);
            }

            public IObservableMessage<Letter> Dequeue()
            {
                BasicDeliverEventArgs deliverEventArgs = _consumer.Queue.Dequeue();
                return new ObservableMessage(deliverEventArgs, _channel);
            }

            public void Dispose()
            {
                if (_channel != null)
                {
                    _channel.Dispose();
                }
            }

            private sealed class ObservableMessage : IObservableMessage<Letter>
            {
                private readonly IModel _channel;
                private readonly BasicDeliverEventArgs _deliverEventArgs;

                public ObservableMessage(BasicDeliverEventArgs deliverEventArgs, IModel channel)
                {
                    _deliverEventArgs = deliverEventArgs;
                    _channel = channel;

                    byte[] body = deliverEventArgs.Body;
                    string message = Encoding.UTF8.GetString(body);
                    Message = JsonSerializer.DeserializeFromString<Letter>(message);
                }

                public Letter Message { get; private set; }

                public void Completed()
                {
                    _channel.BasicAck(_deliverEventArgs.DeliveryTag, false);
                }

                public void Failed()
                {
                    _channel.BasicReject(_deliverEventArgs.DeliveryTag, true);
                }
            }
        }

        private sealed class IncomingLettersEnqueuer : IIncomingLettersEnqueuer
        {
            private readonly IModel _channel;

            public IncomingLettersEnqueuer(IConnection connection)
            {
                _channel = connection.CreateModel();
                _channel.BasicQos(0, 16*1024, false);
                _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
                _channel.QueueDeclare(QueueName, true, false, false, null);
                _channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);
            }

            public void Enque(Letter letter)
            {
                string json = JsonSerializer.SerializeToString(letter);
                byte[] messageBodyBytes = Encoding.UTF8.GetBytes(json);
                IBasicProperties props = _channel.CreateBasicProperties();
                props.DeliveryMode = 2;
                lock (_channel)
                {
                    _channel.BasicPublish(ExchangeName, RoutingKey, props, messageBodyBytes);
                }
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
}