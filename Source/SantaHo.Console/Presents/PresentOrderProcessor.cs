using SantaHo.Domain.Presents;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.SantaOffice.Service.Presents
{
    public sealed class PresentOrderProcessor : IPresentOrderProcessor
    {
        private readonly IToyOrdersEnqueuer _enqueuer;

        public PresentOrderProcessor(IToyOrdersEnqueuer enqueuer)
        {
            _enqueuer = enqueuer;
        }

        public void Process(PresentOrder order)
        {
            order.ToProduce.ForEach(_enqueuer.Enque);
        }
    }
}