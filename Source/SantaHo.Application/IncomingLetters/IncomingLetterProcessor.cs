using System.Threading;
using NLog;
using SantaHo.Domain.IncomingLetters;

namespace SantaHo.Application.IncomingLetters
{
    public class IncomingLetterProcessor : IIncomingLetterProcessor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private int _processed;

        public void Process(IObservableMessage<Letter> letter)
        {
            int letterNumber = Interlocked.Increment(ref _processed);
            Logger.Info("Processing letter \t{0}: {1}", letterNumber, letter.Message.Name);
            Thread.Sleep(1000);
            letter.Completed();
        }
    }
}