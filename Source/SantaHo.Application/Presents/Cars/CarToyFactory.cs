using SantaHo.Domain.Presents.Cars;

namespace SantaHo.Application.Presents.Cars
{
    public class CarToyFactory : ToyFactory<CarToy>
    {
        protected override CarToy CreateCore()
        {
            return new CarToy
            {
                Model = "Ferrary",
                Color = "Red",
                Weight = 1,
            };
        }
    }
}