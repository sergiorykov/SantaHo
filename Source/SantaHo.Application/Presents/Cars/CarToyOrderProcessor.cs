using System.Threading;
using System.Threading.Tasks;
using SantaHo.Domain.Presents;
using SantaHo.Domain.Presents.Cars;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.Application.Presents.Cars
{
    public class CarToyOrderProcessor
    {
        private readonly IToyOrderDequeuer _dequeuer;
        private readonly ToyFactory<CarToy> _toyFactory;
        private CancellationTokenSource _cancellationTokenSource;

        public CarToyOrderProcessor(IToyOrderDequeuer dequeuer, ToyFactory<CarToy> toyFactory)
        {
            _dequeuer = dequeuer;
            _toyFactory = toyFactory;
        }

        public void Start()
        {
            if (_cancellationTokenSource == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(AwaitAndProcessOrder, _cancellationTokenSource.Token);
            }
        }

        public void Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }
        }

        private void AwaitAndProcessOrder()
        {
            while (true)
            {
                var order = _dequeuer.Dequeue();
                _toyFactory.Create();
            }
            
        }
    }
}