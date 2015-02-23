using FluffyRabbit.Producers;

namespace FluffyRabbit
{
    public static class RabbitQueue
    {
        public static ProducerConfigurator Producer()
        {
            return new ProducerConfigurator();
        }
    }
}