using System;

namespace SantaHo.Domain.IncomingLetters
{
    public interface IIncomingLettersDequeuer : IDisposable
    {
        IObservableMessage<Letter> Dequeue();
    }
}