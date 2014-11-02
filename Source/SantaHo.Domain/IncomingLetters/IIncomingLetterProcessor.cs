using SantaHo.Core.Processing;

namespace SantaHo.Domain.IncomingLetters
{
    public interface IIncomingLetterProcessor
    {
        bool IsBusy { get; }
        void Process(IObservableMessage<Letter> letter);
        void Start();
        void Stop();
    }
}