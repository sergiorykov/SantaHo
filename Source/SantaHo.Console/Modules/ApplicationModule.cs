﻿using Ninject.Modules;
using SantaHo.Application.IncomingLetters;
using SantaHo.Core.ApplicationServices;
using SantaHo.Domain.IncomingLetters;

namespace SantaHo.Console.Modules
{
    public sealed class ApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IIncomingLetterProcessor>().To<IncomingLetterProcessor>().InSingletonScope();

            Bind<IApplicationService>()
                .To<IncomingLettersApplicationService>()
                .InSingletonScope();
        }
    }
}