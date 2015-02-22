using RabbitMQ.Client;

namespace FluffyRabbit
{
    public sealed class RabbitExchange
    {
        private readonly IModel _channel;
        private readonly string _exchangeName;

        public RabbitExchange(IModel channel, string exchangeName)
        {
            _exchangeName = exchangeName;
            _channel = channel;
        }

        public RabbitQueueBuilder Direct()
        {
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            return CreateBuilder();
        }

        private RabbitQueueBuilder CreateBuilder()
        {
            return new RabbitQueueBuilder(_channel, _exchangeName);
        }

        public RabbitQueueBuilder Fanout()
        {
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout);
            return new RabbitQueueBuilder(_channel, _exchangeName);
        }
    }
}