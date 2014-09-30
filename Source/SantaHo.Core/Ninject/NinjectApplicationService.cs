using Ninject;
using SantaHo.Core.ApplicationServices;

namespace SantaHo.Core.Ninject
{
    public abstract class NinjectApplicationService : IApplicationService
    {
        protected NinjectApplicationService(IKernel kernel)
        {
            Kirnel = kernel;
        }

        protected IKernel Kirnel { get; private set; }

        public abstract void Start();

        public abstract void Stop();
    }
}