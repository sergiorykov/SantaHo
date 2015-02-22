using System;
using System.Threading;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using RabbitMQ.Client;

namespace FluffyRabbit
{
    public sealed class RabbitConnectionFactory
    {
        private Option<ConnectionFactory> _connectionFactory;

        public RabbitConnectionFactory()
        {
            _connectionFactory = Option<ConnectionFactory>.Empty;
        }

        public RabbitConnectionFactory LinkTo(string connectionUri)
        {
            if (string.IsNullOrEmpty(connectionUri))
            {
                throw new ArgumentNullException("connectionUri");
            }

            var factory = new ConnectionFactory
            {
                Uri = connectionUri,
                Protocol = Protocols.DefaultProtocol,
                AutomaticRecoveryEnabled = true
            }.ToOption();

            Interlocked.Exchange(ref _connectionFactory, factory);

            return this;
        }

        public IConnection Create()
        {
            return _connectionFactory
                .ThrowOnEmpty(() => new InvalidOperationException("Factory not connected"))
                .Map(x => x.CreateConnection())
                .Value;
        }
    }
}