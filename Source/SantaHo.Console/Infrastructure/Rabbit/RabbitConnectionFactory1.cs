using System;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using RabbitMQ.Client;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.ApplicationServices.Resources;

namespace SantaHo.Infrastructure.Rabbit
{
    public sealed class RabbitConnectionFactory1 : IRequireLoading
    {
        public const string UriStartupKey = "RabbitMQ:Uri";
        private Option<ConnectionFactory> _connectionFactory;

        public IConnection Create()
        {
            return _connectionFactory
                .ThrowOnEmpty(() => new InvalidOperationException("Load resource first"))
                .Map(x => x.CreateConnection())
                .Value;
        }

        public void Load(IStartupSettings settings)
        {
            // http://www.rabbitmq.com/uri-spec.html
            // http://lostechies.com/derekgreer/2012/03/18/rabbitmq-for-windows-hello-world-review/
            // amqp://guest:guest@localhost:5672/%2f
            var connectionUri = settings.GetValue<string>(UriStartupKey);
            _connectionFactory = new ConnectionFactory
            {
                Uri = connectionUri,
                Protocol = Protocols.DefaultProtocol,
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