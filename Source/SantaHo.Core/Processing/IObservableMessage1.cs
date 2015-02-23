namespace SantaHo.Core.Processing
{
    public interface IObservableMessage1<out TMessage>
    {
        TMessage Message { get; }
        void Completed();
        void Failed();
    }
}