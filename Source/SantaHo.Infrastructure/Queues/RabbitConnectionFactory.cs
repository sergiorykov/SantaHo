using RabbitMQ.Client;

namespace SantaHo.Infrastructure.Queues
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
                Protocol = Protocols.FromEnvironment(),
                HostName = "localhost",
                Port = AmqpTcpEndpoint.UseDefaultPort
            };
            return factory.CreateConnection();
        } 
    }
}