using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SantaHo.Core.Processing;
using SantaHo.Domain.Letters;
using SantaHo.Infrastructure.Core.Constants;
using SantaHo.Infrastructure.Rabbit;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public sealed class IncomingLettersQueueManager
    {
        private readonly RabbitConnectionFactory1 _connectionFactory;

        public IncomingLettersQueueManager(RabbitConnectionFactory1 connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        
        private sealed class IncomingLettersDequeuer1
        {
            private readonly IModel _channel;
            private readonly QueueingBasicConsumer _consumer;

            public IncomingLettersDequeuer1(IConnection connection)
            {
                _channel = connection.CreateModel();
                _channel.QueueDeclare(QueueKeys.IncomingLetters.QueueName, true, false, false, null);
                _channel.BasicQos(0, 16*1024, false);

                _consumer = new QueueingBasicConsumer(_channel);
                _channel.BasicConsume(QueueKeys.IncomingLetters.QueueName, false, _consumer);
            }

            public IObservableMessage1<Letter> Dequeue()
            {
                BasicDeliverEventArgs deliverEventArgs = _consumer.Queue.Dequeue();
                return new ObservableMessage1(deliverEventArgs, _channel);
            }

            public void Dispose()
            {
                if (_channel != null)
                {
                    _channel.Dispose();
                }
            }

            private sealed class ObservableMessage1 : IObservableMessage1<Letter>
            {
                private readonly IModel _channel;
                private readonly BasicDeliverEventArgs _deliverEventArgs;

                public ObservableMessage1(BasicDeliverEventArgs deliverEventArgs, IModel channel)
                {
                    _deliverEventArgs = deliverEventArgs;
                    _channel = channel;

                    byte[] body = deliverEventArgs.Body;
                    string message = Encoding.UTF8.GetString(body);
                    Message = JsonConvert.DeserializeObject<Letter>(message);
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

        
    }
}