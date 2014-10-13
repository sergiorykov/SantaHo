using SantaHo.Core.Processing;
using SantaHo.Domain.IncomingLetters;

namespace SantaHo.Application.IncomingLetters
{
    public class ProcessIncomingLetterTaskFactory : ITaskFactory<IObservableMessage<Letter>, ProcessIncomingLetterSantaTask>
    {
        public ProcessIncomingLetterSantaTask Create(IObservableMessage<Letter> letter)
        {
            return new ProcessIncomingLetterSantaTask(letter);
        }
    }
}