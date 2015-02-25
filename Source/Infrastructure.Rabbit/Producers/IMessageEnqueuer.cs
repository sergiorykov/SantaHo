using System;

namespace FluffyRabbit.Producers
{
    public interface IMessageEnqueuer<in TMessage> : IDisposable
    {
        void Enque(TMessage message);
    }
}
