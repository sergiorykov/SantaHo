using SantaHo.Domain.IncomingLetters;

namespace SantaHo.FrontEnd.Service.Queues
{
    public interface IIncomingLetterQueue
    {
        void Enqueue(Letter letter);
    }
}