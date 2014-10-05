using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SantaHo.Domain.IncomingLetters;
using ServiceStack.Text;
using System.Text;

namespace SantaHo.Infrastructure.Queues
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

        private sealed class IncomingLettersEnqueuer : IIncomingLettersEnqueuer
        {
            private readonly IModel _channel;

            public IncomingLettersEnqueuer(IConnection connection)
            {
                _channel = connection.CreateModel();
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
        
        private sealed class IncomingLettersDequeuer : IIncomingLettersDequeuer
        {
            private readonly QueueingBasicConsumer _consumer;
            private readonly IModel _channel;

            public IncomingLettersDequeuer(IConnection connection)
            {
                _channel = connection.CreateModel();
                _channel.QueueDeclare(QueueName, true, false, false, null);
                _consumer = new QueueingBasicConsumer(_channel);
                _channel.BasicConsume(QueueName, true, _consumer);
            }
            
            public Letter Dequeue()
            {
                BasicDeliverEventArgs deliverEventArgs = _consumer.Queue.Dequeue();
                byte[] body = deliverEventArgs.Body;
                string message = Encoding.UTF8.GetString(body);
                return JsonSerializer.DeserializeFromString<Letter>(message);
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