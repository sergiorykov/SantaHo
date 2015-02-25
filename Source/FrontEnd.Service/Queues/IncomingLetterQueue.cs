using System;
using FluffyRabbit;
using FluffyRabbit.Exchanges;
using FluffyRabbit.Producers;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using RabbitMQ.Client;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Core.Exceptions;
using SantaHo.Domain.SantaOffice.Letters;
using SantaHo.Infrastructure.Core.Constants;

namespace SantaHo.FrontEnd.Service.Queues
{
    public sealed class IncomingLetterQueue : IIncomingLetterQueue, IApplicationResource
    {
        private Option<IMessageEnqueuer<IncomingChildLetter>> _letterEnqueuer = Option<IMessageEnqueuer<IncomingChildLetter>>.Empty;
        private readonly QueueConnectionManager _manager;

        public IncomingLetterQueue(QueueConnectionManager manager)
        {
            _manager = manager;
        }

        public void Dispose()
        {
            _letterEnqueuer.Do(x => x.Dispose());
        }

        public void Load(IStartupSettings startupSettings)
        {
            IConnection connection = _manager.GetConnection();
            IMessageEnqueuer<IncomingChildLetter> letterEnqueuer = RabbitQueue.Producer()
                                                                              .UseExchange(x =>
                                                                              {
                                                                                  x.Name = QueueKeys.IncomingLetters.ExchangeName;
                                                                                  x.Type = RabbitExchangeType.Direct;
                                                                              })
                                                                              .Queue(x =>
                                                                              {
                                                                                  x.Name = QueueKeys.IncomingLetters.QueueName;
                                                                                  x.Durable = true;
                                                                                  x.PrefetchCount = 16 * 1000;
                                                                                  x.RoutingKey = QueueKeys.IncomingLetters.RoutingKey;
                                                                              })
                                                                              .Create<IncomingChildLetter>(connection);

            _letterEnqueuer = letterEnqueuer.ToOption();
        }

        public void Enqueue(IncomingChildLetter letter)
        {
            letter.ToOption()
                  .Do(x => x.Validate());

            _letterEnqueuer
                .ThrowOnEmpty(() => new QueueUnavailableException())
                .Do(x => x.Enque(letter));
        }
    }
}
