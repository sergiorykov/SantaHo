using SantaHo.Core.Processing;
using SantaHo.Domain.IncomingLetters;

namespace SantaHo.Application.IncomingLetters
{
    public class ProcessIncomingLetterTaskFactory : ITaskFactory<IObservableMessage<Letter>, ProcessIncomingLetterTask>
    {
        public ProcessIncomingLetterTask Create(IObservableMessage<Letter> letter)
        {
            return new ProcessIncomingLetterTask(letter);
        }
    }
}