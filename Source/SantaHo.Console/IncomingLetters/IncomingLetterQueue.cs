using FluffyRabbit;
using FluffyRabbit.Consumers;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Domain.Letters;
using SantaHo.SantaOffice.Service.Infrastructure.Queues;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public class IncomingLetterQueue : IApplicationResource
    {
        private readonly IQueueConnectionFactory _connectionFactory;

        public IncomingLetterQueue(IQueueConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IObservableMessageConsumer<Letter> CreateConsumer()
        {
            //RabbitQueue.From(_connectionFactory.Create())
            return null;
        }

        public void Dispose()
        {
        }

        public void Load(IStartupSettings startupSettings)
        {
        }
    }
}