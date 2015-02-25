namespace SantaHo.Domain.Presents.Cars
{
    public sealed class CarToyFactory : ToyFactory<CarToy>
    {
        public const string Category = "car";

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