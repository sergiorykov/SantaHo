using SantaHo.Domain.Presents;

namespace SantaHo.SantaOffice.Service.Presents
{
    public interface IPresentOrderProcessor
    {
        void Process(PresentOrder order);
    }
}