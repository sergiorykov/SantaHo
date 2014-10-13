using System;
using SantaHo.Core.Processing;

namespace SantaHo.Domain.IncomingLetters
{
    public interface IIncomingLettersDequeuer : IDisposable
    {
        IObservableMessage<Letter> Dequeue();
    }
}