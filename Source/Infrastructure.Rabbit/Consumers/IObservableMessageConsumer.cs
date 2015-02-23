using System;

namespace FluffyRabbit.Consumers
{
    public interface IObservableMessageConsumer<out TMessage> : IDisposable
    {
        IObservableMessage<TMessage> Dequeue();
    }

    public interface IObservableMessage<out TMessage>
    {
        TMessage Message { get; }
        void Completed();
        void Failed();
    }
}