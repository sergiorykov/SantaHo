using System;

namespace FluffyRabbit.Queues
{
    public interface IRabbitQueueConfiguration
    {
        bool Durable { get; set; }
        bool AutoDelete { get; set; }
        ushort PrefetchCount { get; set; }
        string RoutingKey { get; set; }
        string Name { get; set; }
    }
}
