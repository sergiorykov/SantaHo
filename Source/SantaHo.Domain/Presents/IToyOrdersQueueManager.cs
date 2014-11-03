using SantaHo.Domain.SantaOffice;

namespace SantaHo.Domain.Presents
{
    public interface IToyOrdersQueueManager
    {
        IToyOrderDequeuer GetDequeuer(string category);
    }
}