using SantaHo.Core.Processing;

namespace SantaHo.Domain.IncomingLetters
{
    public interface IIncomingLetterProcessor
    {
        void Process(IObservableMessage<Letter> letter);
        void Start();
        void Stop();
        bool IsBusy { get; }
    }
}