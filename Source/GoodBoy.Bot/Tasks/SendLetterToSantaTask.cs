using FluentScheduler;
using GoodBoy.Bot.Clients;
using SantaHo.Domain.IncomingLetters;

namespace GoodBoy.Bot.Tasks
{
    public sealed class SendLetterToSantaTask : ITask
    {
        private readonly LetterProvider _provider;
        private readonly ISantaPostOfficeClient _santaPostOffice;

        public SendLetterToSantaTask(LetterProvider provider, ISantaPostOfficeClient santaPostOffice)
        {
            _provider = provider;
            _santaPostOffice = santaPostOffice;
        }

        public void Execute()
        {
            Letter letter = _provider.Create();
            _santaPostOffice.Send(letter);
        }
    }
}