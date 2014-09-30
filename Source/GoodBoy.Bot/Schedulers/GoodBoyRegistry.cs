using FluentScheduler;
using GoodBoy.Bot.Tasks;

namespace GoodBoy.Bot.Schedulers
{
    public class GoodBoyRegistry : Registry
    {
        public GoodBoyRegistry()
        {
            Schedule<SendLetterToSantaTask>().ToRunNow().AndEvery(2).Seconds();
        }
    }
}