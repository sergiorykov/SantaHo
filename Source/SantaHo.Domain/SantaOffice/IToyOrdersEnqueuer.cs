using System;

namespace SantaHo.Domain.SantaOffice
{
    public interface IToyOrdersEnqueuer : IDisposable
    {
        void Enque(ToyOrder order);
    }
}