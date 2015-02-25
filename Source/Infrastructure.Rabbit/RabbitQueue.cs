using System;
using FluffyRabbit.Consumers;
using FluffyRabbit.Producers;

namespace FluffyRabbit
{
    public static class RabbitQueue
    {
        public static ProducerConfigurator Producer()
        {
            return new ProducerConfigurator();
        }

        public static ConsumerConfigurator Consumer()
        {
            return new ConsumerConfigurator();
        }
    }
}
