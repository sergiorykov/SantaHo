using FluentScheduler;
using GoodBoy.Bot.Clients;
using GoodBoy.Bot.Providers;
using SantaHo.ServiceContracts.Letters;

namespace GoodBoy.Bot.Tasks
{
    public sealed class StressTestSantaOfficeTask : ITask
    {
        private readonly WishRequestProvider _provider;
        private readonly ISantaPostOfficeClient _santaPostOffice;

        public StressTestSantaOfficeTask(WishRequestProvider provider, ISantaPostOfficeClient santaPostOffice)
        {
            _provider = provider;
            _santaPostOffice = santaPostOffice;
        }

        public void Execute()
        {
            while (true)
            {
                WishListLetterRequest request = _provider.Create();
                _santaPostOffice.Send(request);
            }
        }
    }
}