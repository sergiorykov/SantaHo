using System;

namespace FluffyRabbit
{
    public interface IMessageEnqueuer<in TMessage> : IDisposable
    {
        void Enque(TMessage message);
    }
}