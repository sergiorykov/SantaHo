using SantaHo.Domain.SantaOffice.Letters;

namespace SantaHo.FrontEnd.Service.Queues
{
    public interface IIncomingLetterQueue
    {
        void Enqueue(IncomingChildLetter letter);
    }
}