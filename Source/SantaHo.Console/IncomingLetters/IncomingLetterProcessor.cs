using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SantaHo.Core.Configuration;
using SantaHo.Core.Processing;
using SantaHo.Domain.IncomingLetters;

namespace SantaHo.SantaOffice.Service.IncomingLetters
{
    public class IncomingLetterProcessor : IIncomingLetterProcessor, ISupportSettingsMigration
    {
        private readonly ConcurrentQueue<ProcessIncomingLetterTask> _queue =
            new ConcurrentQueue<ProcessIncomingLetterTask>();

        private readonly ISettingsRepository _settingsRepository;
        private readonly ProcessIncomingLetterTaskFactory _taskFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private int _processed;
        private IncomingLetterProcessingSettings _settings = IncomingLetterProcessingSettings.Default;

        public IncomingLetterProcessor(
            ProcessIncomingLetterTaskFactory taskFactory, ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            _taskFactory = taskFactory;
        }

        public void Process(IObservableMessage<Letter> letter)
        {
            int letterNumber = Interlocked.Increment(ref _processed);

            ProcessIncomingLetterTask task = _taskFactory.Create(letter);
            _queue.Enqueue(task);
        }

        public void Start()
        {
            _settings = _settingsRepository.Get<IncomingLetterProcessingSettings>();
            _cancellationTokenSource = StartProcessing();
        }

        public void Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }

            AbortAllPendingTasksInQueue();
        }

        public bool IsBusy
        {
            get { return _settings.MaxAwaiting < _queue.Count; }
        }

        public void PrepareSettings(ISettingsMigrationRegistrar registrar)
        {
            registrar.Register(IncomingLetterProcessingSettings.Default);
        }

        private CancellationTokenSource StartProcessing()
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
                ProcessIncomingLetterTask task;
                if (_queue.TryDequeue(out task))
                {
                    task.Execute();
                }
            }
        }

        private void AbortAllPendingTasksInQueue()
        {
            while (!_queue.IsEmpty)
            {
                ProcessIncomingLetterTask task;
                if (_queue.TryDequeue(out task))
                {
                    task.Abort();
                }
            }
        }
    }
}