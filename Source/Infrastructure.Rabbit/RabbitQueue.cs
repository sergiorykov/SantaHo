using System;
using RabbitMQ.Client;

namespace SantaHo.Infrastructure.Rabbit
{
    public sealed class RabbitQueue
    {
        private readonly IModel _channel;

        public RabbitQueue(IModel channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException("channel");
            }

            _channel = channel;
        }

        public RabbitExchange Exchange(string exchangeName)
        {
            return new RabbitExchange(_channel, exchangeName);
        }
    }
}