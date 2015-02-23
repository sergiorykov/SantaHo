using System;
using SantaHo.Core.Processing;

namespace SantaHo.Domain.SantaOffice
{
    public interface IToyOrderDequeuer : IDisposable
    {
        IObservableMessage1<ToyOrder> Dequeue();
    }
}