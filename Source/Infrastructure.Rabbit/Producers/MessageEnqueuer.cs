using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FluffyRabbit.Producers
{
    internal sealed class MessageEnqueuer<TMessage> : IMessageEnqueuer<TMessage>
    {
        private readonly IModel _channel;
        private readonly string _exchangeName;
        private readonly string _routingKey;

        public MessageEnqueuer(IModel channel, string exchangeName, string routingKey = null)
        {
            _channel = channel;
            _exchangeName = exchangeName;
            _routingKey = routingKey;
        }

        public void Dispose()
        {
            _channel.Dispose();
        }

        public void Enque(TMessage message)
        {
            var messageBody = GetBody(message);
            var basicProperties = _channel.CreateBasicProperties();
            basicProperties.SetPersistent(true);
            _channel.BasicPublish(_exchangeName, _routingKey, basicProperties, messageBody);
        }

        private static byte[] GetBody(TMessage message)
        {
            var json = JsonConvert.SerializeObject(message);
            var messageBodyBytes = Encoding.UTF8.GetBytes(json);
            return messageBodyBytes;
        }
    }
}