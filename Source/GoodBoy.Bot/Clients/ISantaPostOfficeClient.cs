using SantaHo.ServiceContracts.Letters;

namespace GoodBoy.Bot.Clients
{
    public interface ISantaPostOfficeClient
    {
        bool Send(WishListLetterRequest request);
    }
}