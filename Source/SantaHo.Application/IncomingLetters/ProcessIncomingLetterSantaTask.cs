using System.Threading;
using NLog;
using SantaHo.Core.Processing;
using SantaHo.Domain.IncomingLetters;

namespace SantaHo.Application.IncomingLetters
{
    public class ProcessIncomingLetterSantaTask : SantaTask<IObservableMessage<Letter>>
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public ProcessIncomingLetterSantaTask(IObservableMessage<Letter> letter): base(letter)
        {
        }

        protected override void ExecuteCore()
        {
            Thread.Sleep(100);
            Logger.Info("Processing letter: {0}", Value.Message.Name);
            Value.Completed();
        }

        public void Abort()
        {
            Value.Failed();
        }
    }
}