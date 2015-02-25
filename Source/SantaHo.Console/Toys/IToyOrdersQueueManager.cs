using System;

namespace SantaHo.SantaOffice.Service.Toys
{
    public interface IToyOrdersQueueManager
    {
        IToyOrderDequeuer GetDequeuer(string category);
    }
}
