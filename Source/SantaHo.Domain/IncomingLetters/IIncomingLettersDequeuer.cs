using System;
using SantaHo.Core.Queues;

namespace SantaHo.Domain.IncomingLetters
{
    public interface IIncomingLettersDequeuer : IDisposable
    {
        IObservableMessage<Letter> Dequeue();
    }
}