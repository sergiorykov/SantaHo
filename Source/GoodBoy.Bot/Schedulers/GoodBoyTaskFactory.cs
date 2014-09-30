using FluentScheduler;
using Ninject;

namespace GoodBoy.Bot.Schedulers
{
    public class GoodBoyTaskFactory : ITaskFactory
    {
        private readonly IKernel _kernel;

        public GoodBoyTaskFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public ITask GetTaskInstance<T>() where T : ITask
        {
            return _kernel.Get<T>();
        }
    }
}