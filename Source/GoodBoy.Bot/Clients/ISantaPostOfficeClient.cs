using System.Threading.Tasks;
using SantaHo.ServiceContracts.Letters;

namespace GoodBoy.Bot.Clients
{
    public interface ISantaPostOfficeClient
    {
        Task<bool> Send(WishListLetterRequest request);
    }
}