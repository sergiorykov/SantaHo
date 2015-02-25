using System;
using FluffyRabbit;
using FluffyRabbit.Consumers;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Domain.SantaOffice.Letters;
using SantaHo.Infrastructure.Core.Constants;
using SantaHo.SantaOffice.Service.Infrastructure.Queues;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public abstract class IncomingLetterQueue : IApplicationResource
    {
        private readonly IQueueConnectionFactory _connectionFactory;

        public IncomingLetterQueue(IQueueConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Dispose()
        {
        }

        public void Load(IStartupSettings startupSettings)
        {
        }

        public IObservableMessageDequeuer<IncomingChildLetter> CreateConsumer()
        {
            return RabbitQueue.Consumer()
                              .Queue(x =>
                              {
                                  x.Name = QueueKeys.IncomingLetters.QueueName;
                                  x.Durable = true;
                                  x.PrefetchCount = 16 * 1000;
                              })
                              .Create<IncomingChildLetter>(_connectionFactory.Create());
        }
    }
}
