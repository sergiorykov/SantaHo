using NLog;
using SantaHo.Domain.IncomingLetters;
using ServiceStack.Text;

namespace SantaHo.Application.IncomingLetters
{
    public class IncomingLetterProcessor : IIncomingLetterProcessor
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void Process(Letter letter)
        {
            Logger.Info("Processing letter {0}", letter.Dump());
        }
    }
}