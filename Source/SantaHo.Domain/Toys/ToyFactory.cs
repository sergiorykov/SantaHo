using System;

namespace SantaHo.Domain.Presents
{
    public abstract class ToyFactory<TToy>
        where TToy : Toy
    {
        public TToy Create()
        {
            return CreateCore();
        }

        protected abstract TToy CreateCore();
    }
}
