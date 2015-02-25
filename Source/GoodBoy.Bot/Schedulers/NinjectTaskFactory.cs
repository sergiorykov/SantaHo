using System;
using FluentScheduler;
using Ninject;

namespace GoodBoy.Bot.Schedulers
{
    public class NinjectTaskFactory : ITaskFactory
    {
        private readonly IKernel _kernel;

        public NinjectTaskFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public ITask GetTaskInstance<T>() where T : ITask
        {
            return _kernel.Get<T>();
        }
    }
}
