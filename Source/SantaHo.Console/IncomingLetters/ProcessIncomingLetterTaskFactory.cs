using SantaHo.Core.Processing;
using SantaHo.Domain.IncomingLetters;
using SantaHo.Domain.Presents;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public class ProcessIncomingLetterTaskFactory : ITaskFactory<IObservableMessage<Letter>, ProcessIncomingLetterTask>
    {
        private readonly IPresentOrderProcessor _presentOrderProcessor;
        private readonly Santa _santa;

        public ProcessIncomingLetterTaskFactory(Santa santa, IPresentOrderProcessor presentOrderProcessor)
        {
            _santa = santa;
            _presentOrderProcessor = presentOrderProcessor;
        }

        public ProcessIncomingLetterTask Create(IObservableMessage<Letter> letter)
        {
            return new ProcessIncomingLetterTask(letter, _santa, _presentOrderProcessor);
        }
    }
}