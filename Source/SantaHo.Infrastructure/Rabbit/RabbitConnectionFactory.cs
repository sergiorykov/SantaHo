using System;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using RabbitMQ.Client;
using SantaHo.Core.ApplicationServices.Resources;

namespace SantaHo.Infrastructure.Rabbit
{
    public sealed class RabbitConnectionFactory : IRequireLoading
    {
        private Option<ConnectionFactory> _connectionFactory;

        public IConnection Create()
        {
            return _connectionFactory
                .ThrowOnEmpty(() => new InvalidOperationException("Load resource first"))
                .Map(x => x.CreateConnection())
                .Value;
        }

        public void Load()
        {
            _connectionFactory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                Protocol = Protocols.DefaultProtocol,
                HostName = "localhost",
                Port = AmqpTcpEndpoint.UseDefaultPort
            }.ToOption();
        }

        /// <summary>
        /// ¬ыполн€ет определ€емые приложением задачи, св€занные с высвобождением или сбросом неуправл€емых ресурсов.
        /// </summary>
        public void Dispose()
        {
            _connectionFactory = Option<ConnectionFactory>.Empty;
        }
    }
}