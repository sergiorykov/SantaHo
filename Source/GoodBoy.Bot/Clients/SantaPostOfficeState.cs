using System;

namespace GoodBoy.Bot.Clients
{
    public class SantaPostOfficeState
    {
        public SantaPostOfficeState()
        {
            LastSeenAt = DateTimeOffset.UtcNow;
            IsOpen = false;
        }

        public DateTimeOffset LastSeenAt { get; private set; }

        public bool IsOpen { get; set; }
    }
}