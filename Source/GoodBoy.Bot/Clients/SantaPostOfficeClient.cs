using NLog;
using SantaHo.Domain.IncomingLetters;
using ServiceStack.Text;

namespace GoodBoy.Bot.Clients
{
    public class SantaPostOfficeClient : ISantaPostOfficeClient
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void Send(Letter letter)
        {
            Logger.Debug("Letter sent {0}", letter.Dump());
        }
    }
}