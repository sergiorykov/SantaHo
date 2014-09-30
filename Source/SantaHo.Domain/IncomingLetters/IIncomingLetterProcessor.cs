namespace SantaHo.Domain.IncomingLetters
{
    public interface IIncomingLetterProcessor
    {
        void Process(Letter letter);
    }
}