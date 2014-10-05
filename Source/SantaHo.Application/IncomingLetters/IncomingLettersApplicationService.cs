using System;
using System.Threading.Tasks;
using NLog;
using SantaHo.Core.ApplicationServices;
using SantaHo.Core.Queues;
using SantaHo.Domain.IncomingLetters;

namespace SantaHo.Application.IncomingLetters
{
    public class IncomingLettersApplicationService : IApplicationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IIncomingLettersDequeuer _dequeuer;
        private readonly IIncomingLetterProcessor _processor;
        private Task _waitingNewLetters;

        public IncomingLettersApplicationService(
            IIncomingLetterProcessor processor,
            IIncomingLettersDequeuer dequeuer)
        {
            _processor = processor;
            _dequeuer = dequeuer;
        }

        public void Start()
        {
            _waitingNewLetters = Task.Factory.StartNew(ProcessAwaitingLetters);
        }

        public void Stop()
        {
            if (_waitingNewLetters != null)
            {
                _waitingNewLetters.Dispose();
                _waitingNewLetters = null;
            }

            if (_dequeuer != null)
            {
                _dequeuer.Dispose();
            }
        }

        private void ProcessAwaitingLetters()
        {
            while (true)
            {
                WaitAndProcessNext();
            }
        }

        private void WaitAndProcessNext()
        {
            IObservableMessage<Letter> letter = null;
            try
            {
                letter = _dequeuer.Dequeue();
                _processor.Process(letter);
            }
            catch (Exception e)
            {
                if (letter != null)
                {
                    letter.Failed();
                }

                Logger.Warn(e);
            }
        }
    }
}