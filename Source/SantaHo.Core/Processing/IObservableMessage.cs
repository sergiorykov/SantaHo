namespace SantaHo.Core.Processing
{
    public interface IObservableMessage<out TMessage>
    {
        TMessage Message { get; }
        void Completed();
        void Failed();
    }
}