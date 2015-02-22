using FluffyRabbit;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using RabbitMQ.Client;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Core.Exceptions;
using SantaHo.Domain.IncomingLetters;
using SantaHo.Infrastructure.Core.Constants;

namespace SantaHo.FrontEnd.Service.Queues
{
    public sealed class IncomingLetterQueue : IIncomingLetterQueue, IApplicationResource
    {
        private Option<IConnection> _connection = Option<IConnection>.Empty;
        private Option<IMessageEnqueuer<Letter>> _letterEnqueuer = Option<IMessageEnqueuer<Letter>>.Empty;

        public void Dispose()
        {
            _connection.Do(x => x.Dispose());
        }

        public void Load(IStartupSettings startupSettings)
        {
            var queueSettings = QueueSettings.Create(startupSettings);

            var connection = new RabbitConnectionFactory()
                .LinkTo(queueSettings.RabbitAmqpUri)
                .Create();

            _connection = connection.ToOption();

            var letterEnqueuer = new RabbitQueue(connection.CreateModel())
                .Exchange(QueueKeys.IncomingLetters.ExchangeName)
                .Direct()
                .ForQueue(QueueKeys.IncomingLetters.QueueName)
                .Durable()
                .PrefetchMax(16*1000)
                .USeRoutingKey(QueueKeys.IncomingLetters.RoutingKey)
                .CreateEnqueuer<Letter>();

            _letterEnqueuer = letterEnqueuer.ToOption();
        }

        public void Enqueue(Letter letter)
        {
            letter.ToOption()
                .Do(x => x.Validate());

            _letterEnqueuer
                .ThrowOnEmpty(() => new QueueUnavailableException(QueueKeys.IncomingLetters.ExchangeName))
                .Do(x => x.Enque(letter));
        }
    }
}