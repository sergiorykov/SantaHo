using System;
using System.Linq;
using FluentScheduler;
using GoodBoy.Bot.Tasks;

namespace GoodBoy.Bot.Schedulers
{
    public class GoodBoyRegistry : Registry
    {
        public GoodBoyRegistry(int useProcessors)
        {
            Enumerable.Range(1, Math.Min(useProcessors, Environment.ProcessorCount))
                .ToList()
                .ForEach(x => ScheduleLetterSending());
            
        }

        private void ScheduleLetterSending()
        {
            Schedule<SendLetterToSantaTask>().ToRunNow().AndEvery(1).Seconds();
        }
    }
}