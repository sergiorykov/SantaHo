using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SantaHo.Core.Extensions;
using SantaHo.Core.Processing;
using SantaHo.Domain.Presents;
using SantaHo.Domain.Presents.Cars;
using SantaHo.Domain.SantaOffice;
using SantaHo.SantaOffice.Service.Toys;

namespace SantaHo.SantaOffice.Service.Infrastructure.Rabbit.Queues
{
    public sealed class ToyOrdersQueueManager : IToyOrderCategoryRegistrar, IToyOrdersQueueManager
    {
        private const string ExchangeName = "dispatch-toy-orders";
        private const string QueueNamePrefix = "make-toy-";

        private readonly Dictionary<string, string> _categories =
            new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        private readonly RabbitConnectionFactory1 _connectionFactory;

        public ToyOrdersQueueManager(RabbitConnectionFactory1 connectionFactory)
        {
            _connectionFactory = connectionFactory;
            RegisterCategory(CarToyFactory.Category);
        }

        public void RegisterCategory(string category)
        {
            _categories[category] = category.ToLowerInvariant();
        }

        public IToyOrderDequeuer GetDequeuer(string category)
        {
            if (!_categories.ContainsKey(category))
            {
                throw new ArgumentOutOfRangeException("category");
            }

            return new ToyOrderDequeuer(_connectionFactory.Create(), category);
        }

        public IToyOrdersEnqueuer GetEnqueuer()
        {
            return new ToyOrdersEnqueuer(_connectionFactory.Create(), _categories.Values);
        }

        private static string CreateQueueNameByCategory(string category)
        {
            return "{0}{1}".FormatWith(QueueNamePrefix, category.ToLowerInvariant());
        }

        private sealed class ToyOrderDequeuer : IToyOrderDequeuer
        {
            private readonly IModel _channel;
            private readonly QueueingBasicConsumer _consumer;

            public ToyOrderDequeuer(IConnection connection, string category)
            {
                string queueName = CreateQueueNameByCategory(category);

                _channel = connection.CreateModel();
                _channel.QueueDeclare(queueName, true, false, false, null);
                _channel.BasicQos(0, 16*1024, false);
                _consumer = new QueueingBasicConsumer(_channel);
                _channel.BasicConsume(queueName, false, _consumer);
            }

            public IObservableMessage1<ToyOrder> Dequeue()
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

            private sealed class ObservableMessage1 : IObservableMessage1<ToyOrder>
            {
                private readonly IModel _channel;
                private readonly BasicDeliverEventArgs _deliverEventArgs;

                public ObservableMessage1(BasicDeliverEventArgs deliverEventArgs, IModel channel)
                {
                    _deliverEventArgs = deliverEventArgs;
                    _channel = channel;

                    byte[] body = deliverEventArgs.Body;
                    string message = Encoding.UTF8.GetString(body);
                    Message = JsonConvert.DeserializeObject<ToyOrder>(message);
                }

                public ToyOrder Message { get; private set; }

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

        private sealed class ToyOrdersEnqueuer : IToyOrdersEnqueuer
        {
            private readonly IModel _channel;

            public ToyOrdersEnqueuer(IConnection connection, IEnumerable<string> categories)
            {
                _channel = connection.CreateModel();
                _channel.BasicQos(0, 16*1024, false);
                _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);

                foreach (string category in categories)
                {
                    string queueName = CreateQueueNameByCategory(category);
                    _channel.QueueDeclare(queueName, true, false, false, null);
                    _channel.QueueBind(queueName, ExchangeName, category, null);
                }
            }

            public void Enque(ToyOrder order)
            {
                string json = JsonConvert.SerializeObject(order);
                byte[] messageBodyBytes = Encoding.UTF8.GetBytes(json);

                IBasicProperties props = _channel.CreateBasicProperties();
                props.DeliveryMode = 2;

                lock (_channel)
                {
                    _channel.BasicPublish(ExchangeName, order.ToyCategory.ToLowerInvariant(), props, messageBodyBytes);
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