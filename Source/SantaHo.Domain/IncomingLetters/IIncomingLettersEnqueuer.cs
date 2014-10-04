using System;

namespace SantaHo.Domain.IncomingLetters
{
    public interface IIncomingLettersEnqueuer : IDisposable
    {
        void Enque(Letter letter);
    }
}