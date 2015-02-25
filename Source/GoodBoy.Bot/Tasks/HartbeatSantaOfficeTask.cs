using System;
using FluentScheduler;
using GoodBoy.Bot.Clients;

namespace GoodBoy.Bot.Tasks
{
    public sealed class HartbeatSantaOfficeTask : ITask
    {
        private readonly ISantaPostOfficeClient _santaPostOffice;

        public HartbeatSantaOfficeTask(ISantaPostOfficeClient santaPostOffice)
        {
            _santaPostOffice = santaPostOffice;
        }

        public void Execute()
        {
            bool isAlive = _santaPostOffice.Hartbeat();
            if (!isAlive)
            {
                Environment.Exit(1);
            }
        }
    }
}
