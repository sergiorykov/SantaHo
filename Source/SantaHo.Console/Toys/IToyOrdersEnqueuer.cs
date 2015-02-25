using System;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.SantaOffice.Service.Toys
{
    public interface IToyOrdersEnqueuer : IDisposable
    {
        void Enque(ToyOrder order);
    }
}
