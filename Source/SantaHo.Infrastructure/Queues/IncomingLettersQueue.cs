using System.Text;
using NLog;
using RabbitMQ.Client;
using SantaHo.Domain.IncomingLetters;
using ServiceStack.Text;

namespace SantaHo.Infrastructure.Queues
{
    public class IncomingLettersQueue : IIncomingLettersQueue
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        const string ExchangeName = "incoming-letters-direct-exchange";
        const string QueueName = "incoming-letters";
        const string RoutingKey = "letter";
        private readonly IModel _channel;

        public IncomingLettersQueue(IConnection connection)
        {
            _channel = connection.CreateModel();
        }

        public void Create()
        {
            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(QueueName, false, false, false, null);
            _channel.QueueBind(QueueName, ExchangeName, RoutingKey, null);
        }

        public void Send(Letter letter)
        {
            string json = JsonSerializer.SerializeToString(letter);
            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(json);
            _channel.BasicPublish(ExchangeName, RoutingKey, null, messageBodyBytes);
            Logger.Debug("Sent {0}", letter.Dump());
        }
        
    }
}