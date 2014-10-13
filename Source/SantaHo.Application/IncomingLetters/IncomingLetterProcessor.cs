using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SantaHo.Core.Processing;
using SantaHo.Domain.Configuration;
using SantaHo.Domain.IncomingLetters;

namespace SantaHo.Application.IncomingLetters
{
    public class IncomingLetterProcessor : IIncomingLetterProcessor, ISupportSettingsMigration
    {
        private readonly ConcurrentQueue<ProcessIncomingLetterSantaTask> _queue =
            new ConcurrentQueue<ProcessIncomingLetterSantaTask>();

        private readonly ISettingsRepository _settingsRepository;
        private readonly ITaskFactory<IObservableMessage<Letter>, ProcessIncomingLetterSantaTask> _taskFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private int _processed;
        private IncomingLetterProcessingSettings _settings = IncomingLetterProcessingSettings.Default;

        public IncomingLetterProcessor(
            ISettingsRepository settingsRepository,
            ITaskFactory<IObservableMessage<Letter>, ProcessIncomingLetterSantaTask> taskFactory)
        {
            _settingsRepository = settingsRepository;
            _taskFactory = taskFactory;
        }

        public void Process(IObservableMessage<Letter> letter)
        {
            int letterNumber = Interlocked.Increment(ref _processed);

            ProcessIncomingLetterSantaTask task = _taskFactory.Create(letter);
            _queue.Enqueue(task);
        }

        public void Start()
        {
            _settings = _settingsRepository.Get<IncomingLetterProcessingSettings>();
            _cancellationTokenSource = CreateProcessingTasks();
        }

        public void Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }

            AbortAllPendingInQueue();
        }

        public bool IsBusy
        {
            get { return _settings.MaxAwaiting < _queue.Count; }
        }

        public void PrepareSettings(ISettingsMigrationRegistrar registrar)
        {
            registrar.Register(IncomingLetterProcessingSettings.Default);
        }

        private CancellationTokenSource CreateProcessingTasks()
        {
            var cancellationSource = new CancellationTokenSource();
            int workingThreads = Math.Max(
                _settings.ParallelDegree,
                IncomingLetterProcessingSettings.Default.ParallelDegree);

            Enumerable.Range(0, workingThreads)
                .ToList()
                .ForEach(x =>
                    Task.Factory.StartNew(ProcessTasks, cancellationSource.Token));

            return cancellationSource;
        }

        private void ProcessTasks()
        {
            while (true)
            {
                ProcessIncomingLetterSantaTask task;
                if (_queue.TryDequeue(out task))
                {
                    task.Execute();
                }
            }
        }

        private void AbortAllPendingInQueue()
        {
            while (!_queue.IsEmpty)
            {
                ProcessIncomingLetterSantaTask task;
                if (_queue.TryDequeue(out task))
                {
                    task.Abort();
                }
            }
        }
    }
}