using FluffyRabbit;
using FluffyRabbit.Exchanges;
using FluffyRabbit.Producers;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using RabbitMQ.Client;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Core.Exceptions;
using SantaHo.Domain.Letters;
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
            _letterEnqueuer.Do(x => x.Dispose());
        }

        public void Load(IStartupSettings startupSettings)
        {
            var queueSettings = QueueSettings.Create(startupSettings);

            var connection = new RabbitConnectionFactory()
                .LinkTo(queueSettings.RabbitAmqpUri)
                .Create();

            _connection = connection.ToOption();

            var producer = RabbitQueue.Producer()
                .UseExchange(x =>
                {
                    x.Name = QueueKeys.IncomingLetters.ExchangeName;
                    x.Type = RabbitExchangeType.Direct;
                })
                .Queue(x =>
                {
                    x.Name = QueueKeys.IncomingLetters.QueueName;
                    x.Durable = true;
                    x.PrefetchCount = 16*1000;
                    x.RoutingKey = QueueKeys.IncomingLetters.RoutingKey;
                })
                .Create<Letter>(connection);

            _letterEnqueuer = producer.ToOption();
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