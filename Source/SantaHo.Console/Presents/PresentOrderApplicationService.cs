﻿using System;
using System.Collections.Generic;
using SantaHo.Core.ApplicationServices;
using SantaHo.Domain.Presents;
using SantaHo.Domain.Presents.Cars;
using SantaHo.Infrastructure.Core.Extensions;
using SantaHo.SantaOffice.Service.Toys;

namespace SantaHo.SantaOffice.Service.Presents
{
    public class PresentOrderApplicationService : IApplicationService
    {
        private readonly List<IToyOrderProcessor> _toyOrderProcessors = new List<IToyOrderProcessor>();
        private readonly IToyOrdersQueueManager _toyOrdersQueueManager;

        public PresentOrderApplicationService(IToyOrdersQueueManager toyOrdersQueueManager)
        {
            _toyOrdersQueueManager = toyOrdersQueueManager;
            _toyOrderProcessors.Add(CreateProcessor<CarToyFactory, CarToy>(CarToyFactory.Category));
        }

        public void Start(IStartupSettings startupSettings)
        {
            _toyOrderProcessors.ExecuteAllOrRollback(x => x.Start(), x => x.Stop());
        }

        public void Stop()
        {
            _toyOrderProcessors.ForEach(x => x.IgnoreFailureWhen(p => p.Stop()));
        }

        private IToyOrderProcessor CreateProcessor<TToyFactory, TToy>(string category)
            where TToyFactory : ToyFactory<TToy>, new() where TToy : Toy
        {
            var toyFactory = new TToyFactory();
            IToyOrderDequeuer toyOrderDequeuer = _toyOrdersQueueManager.GetDequeuer(category);
            return new ToyOrderProcessor<TToy>(toyOrderDequeuer, toyFactory);
        }
    }
}
