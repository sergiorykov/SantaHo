namespace SantaHo.Domain.IncomingLetters
{
    public interface IObservableMessage<out TMessage>
    {
        TMessage Message { get; }
        void Completed();
        void Failed();
    }
}