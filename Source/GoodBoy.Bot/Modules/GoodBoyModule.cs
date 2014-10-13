using GoodBoy.Bot.Clients;
using Ninject.Modules;

namespace GoodBoy.Bot.Modules
{
    public class GoodBoyModule : NinjectModule
    {
        public override void Load()
        {
            Bind<WishRequestProvider>().ToSelf();
            Bind<ISantaPostOfficeClient>().To<SantaPostOfficeClient>().InSingletonScope();
        }
    }
}