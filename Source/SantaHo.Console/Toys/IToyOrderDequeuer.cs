using System;
using SantaHo.Core.Processing;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.SantaOffice.Service.Toys
{
    public interface IToyOrderDequeuer : IDisposable
    {
        IObservableMessage1<ToyOrder> Dequeue();
    }
}