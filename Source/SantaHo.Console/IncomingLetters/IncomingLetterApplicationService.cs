using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluffyRabbit.Consumers;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using NLog;
using SantaHo.Core.ApplicationServices;
using SantaHo.Domain.SantaOffice.Letters;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public sealed class IncomingLetterApplicationService : IApplicationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Option<CancellationTokenSource> _tokenSource = Option<CancellationTokenSource>.Empty;
        private readonly IncomingLetterProcessor _processor;
        private readonly IncomingLetterQueue _queue;

        public IncomingLetterApplicationService(IncomingLetterProcessor processor, IncomingLetterQueue queue)
        {
            _processor = processor;
            _queue = queue;
        }

        public void Start(IStartupSettings startupSettings)
        {
            const int consumers = 4;

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            Enumerable.Range(0, consumers).Iter(x => { Task.Factory.StartNew(() => ProcessLetter(tokenSource.Token), tokenSource.Token); });

            _tokenSource = tokenSource.ToOption();
        }

        public void Stop()
        {
            _tokenSource.Do(x => x.Cancel());
        }

        private void ProcessLetter(CancellationToken token)
        {
            using (IObservableMessageDequeuer<IncomingChildLetter> dequeuer = _queue.CreateConsumer())
            {
                while (!token.IsCancellationRequested)
                {
                    IObservableMessage<IncomingChildLetter> message = dequeuer.Dequeue();
                    try
                    {
                        _processor.Process(message.Message);
                        message.Completed();
                    }
                    catch (Exception e)
                    {
                        Logger.Warn("Incoming letter processing failed", e);
                        message.Failed();
                    }
                }
            }
        }
    }
}
