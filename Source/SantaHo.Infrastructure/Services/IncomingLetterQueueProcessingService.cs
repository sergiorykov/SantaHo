using System;
using System.Text;
using System.Threading.Tasks;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SantaHo.Core.ApplicationServices;
using SantaHo.Domain.IncomingLetters;
using ServiceStack.Text;

namespace SantaHo.Infrastructure.Services
{
    public class IncomingLetterQueueProcessingService : IApplicationService
    {
        private const string QueueName = "incoming-letters";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IModel _channel;
        private QueueingBasicConsumer _consumer;
        private Task _waitingNewLetters;
        private readonly IIncomingLetterProcessor _processor;

        public IncomingLetterQueueProcessingService(IConnection connection, IIncomingLetterProcessor processor)
        {
            _processor = processor;
            _channel = connection.CreateModel();
        }

        public void Start()
        {
            _channel.QueueDeclare(QueueName, false, false, false, null);
            _consumer = new QueueingBasicConsumer(_channel);
            _channel.BasicConsume(QueueName, true, _consumer);
            _waitingNewLetters = Task.Factory.StartNew(WaitAndPrepareLetters);
        }

        public void Stop()
        {
            if (_waitingNewLetters != null)
            {
                _waitingNewLetters.Dispose();
                _waitingNewLetters = null;
            }

            if (_channel != null)
            {
                _channel.BasicCancel(_consumer.ConsumerTag);
                _channel.Close();
            }
        }

        private void WaitAndPrepareLetters()
        {
            while (true)
            {
                Letter letter = GetLetter();
                if (letter != null)
                {
                    Process(letter);
                }
            }
        }

        private void Process(Letter letter)
        {
            try
            {
                Logger.Debug("Received {0}", letter.Dump());
                _processor.Process(letter);
            }
            catch (Exception e)
            {
                Logger.Warn(e);
            }
        }

        private Letter GetLetter()
        {
            try
            {
                BasicDeliverEventArgs deliverEventArgs = _consumer.Queue.Dequeue();

                byte[] body = deliverEventArgs.Body;
                string message = Encoding.UTF8.GetString(body);
                return JsonSerializer.DeserializeFromString<Letter>(message);
            }
            catch (Exception e)
            {
                Logger.Warn(e);
            }

            return null;
        }
    }
}