using SantaHo.Core.Queues;

namespace SantaHo.Domain.IncomingLetters
{
    public interface IIncomingLetterProcessor
    {
        void Process(IObservableMessage<Letter> letter);
    }
}