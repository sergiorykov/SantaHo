using Ninject;
using Ninject.Modules;
using SantaHo.Core.ApplicationServices.Resources;
using SantaHo.FrontEnd.Service.Queues;

namespace SantaHo.FrontEnd.Service.Bootstrap
{
    public class QueuesModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicationResource>().ToMethod(ctx => ctx.Kernel.Get<QueueConnectionManager>());
            Bind<QueueConnectionManager>().ToSelf().InSingletonScope();
            
            Bind<IApplicationResource>().ToMethod(ctx => ctx.Kernel.Get<IncomingLetterQueue>());
            Bind<IIncomingLetterQueue>().ToMethod(ctx => ctx.Kernel.Get<IncomingLetterQueue>());
            Bind<IncomingLetterQueue>().ToSelf().InSingletonScope();
        }
    }
}