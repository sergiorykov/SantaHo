using GoodBoy.Bot.Clients;
using GoodBoy.Bot.Tasks;
using Ninject.Modules;

namespace GoodBoy.Bot.Modules
{
    public class GoodBoyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<LetterProvider>().ToSelf();
            Bind<ISantaPostOfficeClient>().To<SantaPostOfficeClient>();
        }
    }
}