using SantaHo.Core.Processing;
using SantaHo.Domain.IncomingLetters;
using SantaHo.Domain.Presents;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public class ProcessIncomingLetterTask : ProcessingTask<IObservableMessage<Letter>>
    {
        private readonly IPresentOrderProcessor _presentOrderProcessor;
        private readonly Santa _santa;

        public ProcessIncomingLetterTask(IObservableMessage<Letter> letter, Santa santa,
                                         IPresentOrderProcessor presentOrderProcessor) : base(letter)
        {
            _santa = santa;
            _presentOrderProcessor = presentOrderProcessor;
        }

        protected override void ExecuteCore()
        {
            PresentOrder order = _santa.Read(Value.Message);
            _presentOrderProcessor.Process(order);
            Value.Completed();
        }

        public void Abort()
        {
            Value.Failed();
        }
    }
}