namespace SantaHo.Domain.IncomingLetters
{
    public interface IIncomingLettersQueue
    {
        void Enque(Letter letter);
    }
}