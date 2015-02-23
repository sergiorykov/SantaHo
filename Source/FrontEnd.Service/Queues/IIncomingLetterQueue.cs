using SantaHo.Domain.Letters;

namespace SantaHo.FrontEnd.Service.Queues
{
    public interface IIncomingLetterQueue
    {
        void Enqueue(Letter letter);
    }
}