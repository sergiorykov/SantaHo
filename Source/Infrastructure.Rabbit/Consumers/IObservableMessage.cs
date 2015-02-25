namespace FluffyRabbit.Consumers
{
    public interface IObservableMessage<out TMessage>
    {
        TMessage Message { get; }
        void Completed();
        void Failed();
    }
}