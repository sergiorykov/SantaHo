using System;
using FluffyRabbit;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using RabbitMQ.Client;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Core.Exceptions;

namespace SantaHo.SantaOffice.Service.Infrastructure.Queues
{
    public interface IQueueConnectionFactory
    {
        IConnection Create();
    }


    public class QueueApplicationResource : IApplicationResource, IQueueConnectionFactory
    {
        private Option<IConnection> _connection = Option<IConnection>.Empty;
        private Option<RabbitConnectionFactory> _connectionFactory = Option<RabbitConnectionFactory>.Empty;

        public void Dispose()
        {
            _connection.Do(x => x.Dispose());
        }

        public void Load(IStartupSettings startupSettings)
        {
            _connectionFactory = new RabbitConnectionFactory().LinkTo("").ToOption();
            _connection = _connectionFactory.Map(x => x.Create());
        }

        public IConnection Create()
        {
            return _connection
                .ThrowOnEmpty(() => new QueueUnavailableException())
                .Value;
        }
    }
}
