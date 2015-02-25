using System;

namespace FluffyRabbit.Consumers
{
    public interface IObservableMessageDequeuer<out TMessage> : IDisposable
    {
        IObservableMessage<TMessage> Dequeue();
    }
}
