using System;
using System.Threading;
using FluentScheduler;
using GoodBoy.Bot.Clients;
using SantaHo.ServiceContracts.Letters;

namespace GoodBoy.Bot.Tasks
{
    public sealed class SendLetterToSantaTask : ITask
    {
        private static readonly TimeSpan OutOfBusiness = TimeSpan.FromSeconds(30);

        private readonly LetterProvider _provider;
        private readonly ISantaPostOfficeClient _santaPostOffice;

        public SendLetterToSantaTask(LetterProvider provider, ISantaPostOfficeClient santaPostOffice)
        {
            _provider = provider;
            _santaPostOffice = santaPostOffice;
        }

        public void Execute()
        {
            while (true)
            {
                EnsureOfficeIsOpen();
                WishListLetterRequest request = _provider.Create();
                _santaPostOffice.Send(request);
            }
        }

        private void EnsureOfficeIsOpen()
        {
//            bool isOpen = SpinWait.SpinUntil(_santaPostOffice.IsOpen, OutOfBusiness);
//            if (!isOpen)
//            {
//                throw new InvalidOperationException("Santa has gone.");
//            }
        }
    }
}