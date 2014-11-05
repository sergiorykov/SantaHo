using RabbitMQ.Client;

namespace SantaHo.Infrastructure.Rabbit
{
    public static class RabbitConnectionFactory
    {
        public static IConnection Connect()
        {
            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                Protocol = Protocols.DefaultProtocol,
                HostName = "localhost",
                Port = AmqpTcpEndpoint.UseDefaultPort
            };
            return factory.CreateConnection();
        }
    }
}