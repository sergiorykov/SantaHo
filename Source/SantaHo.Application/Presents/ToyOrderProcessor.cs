using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SantaHo.Core.Processing;
using SantaHo.Domain.Presents;
using SantaHo.Domain.SantaOffice;

namespace SantaHo.Application.Presents
{
    public class ToyOrderProcessor<TToy> : IToyOrderProcessor where TToy : Toy
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IToyOrderDequeuer _dequeuer;
        private readonly ToyFactory<TToy> _toyFactory;
        private CancellationTokenSource _cancellationTokenSource;

        public ToyOrderProcessor(IToyOrderDequeuer dequeuer, ToyFactory<TToy> toyFactory)
        {
            _dequeuer = dequeuer;
            _toyFactory = toyFactory;
        }

        public void Start()
        {
            if (_cancellationTokenSource == null)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(AwaitDequeueAndProcess, _cancellationTokenSource.Token);
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

        private void AwaitDequeueAndProcess()
        {
            while (true)
            {
                IObservableMessage<ToyOrder> order = _dequeuer.Dequeue();
                Process(order);
            }
        }

        private void Process(IObservableMessage<ToyOrder> order)
        {
            try
            {
                TToy toy = _toyFactory.Create();
                order.Completed();
            }
            catch (Exception e)
            {
                Logger.Warn(e);
                order.Failed();
            }
        }
    }
}