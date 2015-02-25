using System;
using SantaHo.FrontEnd.ServiceContracts.Letters;

namespace GoodBoy.Bot.Clients
{
    public interface ISantaPostOfficeClient
    {
        bool Send(WishListLetterRequest request);
        bool Hartbeat();
    }
}
