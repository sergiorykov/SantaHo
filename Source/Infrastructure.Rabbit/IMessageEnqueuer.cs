using System;

namespace SantaHo.Infrastructure.Rabbit
{
    public interface IMessageEnqueuer<in TMessage> : IDisposable
    {
        void Enque(TMessage message);
    }
}