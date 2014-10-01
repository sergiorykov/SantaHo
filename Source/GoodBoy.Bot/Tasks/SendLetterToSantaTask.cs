using FluentScheduler;
using GoodBoy.Bot.Clients;
using NLog;
using SantaHo.ServiceContracts.Letters;

namespace GoodBoy.Bot.Tasks
{
    public sealed class SendLetterToSantaTask : ITask
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly LetterProvider _provider;
        private readonly ISantaPostOfficeClient _santaPostOffice;

        public SendLetterToSantaTask(LetterProvider provider, ISantaPostOfficeClient santaPostOffice)
        {
            _provider = provider;
            _santaPostOffice = santaPostOffice;
        }

        public void Execute()
        {
            WishListLetterRequest request = _provider.Create();
            bool sent = _santaPostOffice.Send(request).Result;
            if (!sent)
            {
                Logger.Info("Что-то не так с письмом. Печалька.");
            }
        }
    }
}