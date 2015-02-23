using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluffyRabbit;
using FluffyRabbit.Consumers;
using Nelibur.Sword.DataStructures;
using Nelibur.Sword.Extensions;
using NLog;
using SantaHo.Core.ApplicationServices;
using SantaHo.Domain.Letters;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public sealed class IncomingLetterApplicationService : IApplicationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IncomingLetterProcessor _processor;
        private readonly IncomingLetterQueue _queue;
        private Option<CancellationTokenSource> _tokenSource = Option<CancellationTokenSource>.Empty;

        public IncomingLetterApplicationService(
            IncomingLetterProcessor processor,
            IncomingLetterQueue queue)
        {
            _processor = processor;
            _queue = queue;
        }

        public void Start(IStartupSettings startupSettings)
        {
            const int consumers = 4;

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            Enumerable.Range(0, consumers).Iter(x =>
            {
                Task.Factory.StartNew(() => ProcessLetter(tokenSource.Token), tokenSource.Token);
            });

            _tokenSource = tokenSource.ToOption();
        }

        private void ProcessLetter(CancellationToken token)
        {
            using (IObservableMessageConsumer<Letter> consumer = _queue.CreateConsumer())
            {
                while (!token.IsCancellationRequested)
                {
                    IObservableMessage<Letter> message = consumer.Dequeue();
                    try
                    {
                        _processor.Process(message.Message);
                        message.Completed();
                    }
                    catch (Exception e)
                    {
                        Logger.Warn("Incoming letter processing failed", e);
                        message.Completed();
                    }
                }
            }
        }

        public void Stop()
        {
            _tokenSource.Do(x => x.Cancel());
        }
    }
}