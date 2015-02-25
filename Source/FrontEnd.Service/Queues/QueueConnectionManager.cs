using System;
using FluffyRabbit;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using NLog;
using RabbitMQ.Client;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.Core.Exceptions;

namespace SantaHo.FrontEnd.Service.Queues
{
    public sealed class QueueConnectionManager : IApplicationResource
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Option<IConnection> _connection = Option<IConnection>.Empty;

        public void Dispose()
        {
            _connection.Do(x => x.Dispose());
        }

        public void Load(IStartupSettings startupSettings)
        {
            var queueSettings = QueueSettings.Create(startupSettings);

            Logger.Info("Connecting to queue...");
            IConnection connection = new RabbitConnectionFactory()
                .LinkTo(queueSettings.RabbitAmqpUri)
                .Create();

            Logger.Info("Connected to queue {0}", connection.Endpoint);
            _connection = connection.ToOption();
        }

        public IConnection GetConnection()
        {
            return _connection
                .ThrowOnEmpty(() => new QueueUnavailableException())
                .Value;
        }
    }
}
