namespace SantaHo.Domain.Presents
{
    public interface IPresentOrderProcessor
    {
        void Process(PresentOrder order);
        void Start();
        void Stop();
    }
}