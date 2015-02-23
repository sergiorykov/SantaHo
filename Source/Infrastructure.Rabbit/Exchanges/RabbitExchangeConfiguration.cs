using System;
using RabbitMQ.Client;

namespace FluffyRabbit.Exchanges
{
    public sealed class RabbitExchangeConfiguration : IRabbitExchangeConfiguration
    {
        public RabbitExchangeConfiguration()
        {
            Type = RabbitExchangeType.Direct;
        }

        public string Name { get; set; }
        public RabbitExchangeType Type { get; set; }

        private string GetExchangeType()
        {
            switch (Type)
            {
                case RabbitExchangeType.Direct:
                    return ExchangeType.Direct;
                case RabbitExchangeType.Fanout:
                    return ExchangeType.Fanout;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Apply(IModel channel)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidOperationException("Echange Name is required");
            }

            channel.ExchangeDeclare(Name, GetExchangeType());
        }
    }
}