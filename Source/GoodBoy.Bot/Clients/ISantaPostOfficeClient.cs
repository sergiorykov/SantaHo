using SantaHo.Domain.IncomingLetters;

namespace GoodBoy.Bot.Clients
{
    public interface ISantaPostOfficeClient
    {
        void Send(Letter letter);
    }
}