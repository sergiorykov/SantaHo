using System;
using System.Linq;
using FluentScheduler;
using GoodBoy.Bot.Tasks;

namespace GoodBoy.Bot.Schedulers
{
    public class GoodBoyRegistry : Registry
    {
        public GoodBoyRegistry(int bots)
        {
            Enumerable.Range(1, bots)
                .ToList()
                .ForEach(x => ScheduleLetterSending());
            
        }

        private void ScheduleLetterSending()
        {
            Schedule<SendLetterToSantaTask>().ToRunOnceIn(1).Seconds();
        }
    }
}