﻿using Ninject;
using SantaHo.Core.ApplicationServices;

namespace SantaHo.Core.Ninject
{
    public abstract class NinjectApplicationService : IApplicationService
    {
        protected NinjectApplicationService(IKernel kernel)
        {
            Kernel = kernel;
        }

        protected IKernel Kernel { get; private set; }

        public abstract void Start();

        public abstract void Stop();
    }
}