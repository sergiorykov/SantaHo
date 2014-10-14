using System.Threading;
using NLog;
using SantaHo.Core.Processing;
using SantaHo.Domain.IncomingLetters;
using SantaHo.Domain.Presents;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.Application.IncomingLetters
{
    public class ProcessIncomingLetterTask : ProcessingTask<IObservableMessage<Letter>>
    {
        private readonly Santa _santa;
        private readonly IPresentOrderProcessor _presentOrderProcessor;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ProcessIncomingLetterTask(IObservableMessage<Letter> letter, Santa santa, IPresentOrderProcessor presentOrderProcessor): base(letter)
        {
            _santa = santa;
            _presentOrderProcessor = presentOrderProcessor;
        }

        protected override void ExecuteCore()
        {
            Logger.Info("Santa is reading letter from: {0}", Value.Message.From);

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