namespace SantaHo.Domain.IncomingLetters
{
    public interface IIncomingLettersQueue
    {
        void Send(Letter letter);
    }
}