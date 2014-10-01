using System.Linq;
using FluentScheduler;
using GoodBoy.Bot.Tasks;

namespace GoodBoy.Bot.Schedulers
{
    public class GoodBoyRegistry : Registry
    {
        public GoodBoyRegistry(int inParallel)
        {
            Enumerable.Range(1, inParallel)
                .ToList()
                .ForEach(x => ScheduleLetterSending());
            
        }

        private void ScheduleLetterSending()
        {
            Schedule<SendLetterToSantaTask>().ToRunNow().AndEvery(1).Seconds();
        }
    }
}