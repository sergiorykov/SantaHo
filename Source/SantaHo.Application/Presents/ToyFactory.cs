using SantaHo.Domain.Presents;

namespace SantaHo.Application.Presents
{
    public abstract class ToyFactory<TToy> where TToy : Toy
    {
        public TToy Create()
        {
            return CreateCore();
        }

        protected abstract TToy CreateCore();
    }
}