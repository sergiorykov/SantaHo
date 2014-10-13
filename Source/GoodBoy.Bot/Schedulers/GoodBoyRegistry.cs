using System.Linq;
using FluentScheduler;
using GoodBoy.Bot.Clients;
using GoodBoy.Bot.Monitoring;

namespace GoodBoy.Bot.Schedulers
{
    public class GoodBoyRegistry : Registry
    {
        public GoodBoyRegistry(int bots)
        {
            Enumerable.Range(1, bots)
                .ToList()
                .ForEach(x => ScheduleLetterSending());

            Schedule<HartbeatSantaOfficeTask>().ToRunNow().AndEvery(5).Seconds();
        }

        private void ScheduleLetterSending()
        {
            Schedule<StressTestSantaOfficeTask>().ToRunOnceIn(1).Seconds();
        }
    }
}