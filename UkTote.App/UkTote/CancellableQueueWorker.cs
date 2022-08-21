using System.Collections.Concurrent;
using log4net;

namespace UkTote
{
    public abstract class CancellableQueueWorker<T> where T: class
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CancellableQueueWorker<T>));

        public enum RunStateEnum
        {
            NotStarted,
            Starting,
            Started,
            Stopping,
            Stopped
        }

        private readonly List<Task> _taskList;
        private readonly CancellationTokenSource _ctSource = new();
        private CancellationToken _cancellationToken;
        private RunStateEnum _runState = RunStateEnum.NotStarted;

        private readonly ConcurrentQueue<T> _queue = new();
        private readonly ManualResetEventSlim _resetEvent = new(false);
        private readonly int _timeoutMs;
        private readonly int _numThreads;
        private volatile bool _taskComplete;

        public int QueueSize => _queue != null ? _queue.Count : 0;

        public bool Completed => _runState == RunStateEnum.Stopped;

        protected CancellableQueueWorker(int timeoutMs, int numThreads)
        {
            _cancellationToken = _ctSource.Token;
            _timeoutMs = timeoutMs;
            _numThreads = numThreads;
            _taskList = new List<Task>(numThreads);
        }

        public bool TaskComplete 
        {
            get => _taskComplete;
            set
            {
                _taskComplete = value;
                _logger.DebugFormat("TaskComplete({0})", value);
            }
        }

        public void QueueWork(T t)
        {
            _queue.Enqueue(t);
            OnItemQueued(t);
            _resetEvent.Set();
        }

        public bool Start()
        {
            try
            {
                _runState = RunStateEnum.Starting;

                if (!OnStart()) return false;
                for (var i = 0; i < _numThreads; ++i)
                {
                    _taskList.Add(Task.Factory.StartNew(ThreadProc, _cancellationToken));
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Start", ex);
                throw;
            }
        }

        protected void ThreadProc()
        {
            _logger.Debug("ThreadProc starting");
            _cancellationToken.ThrowIfCancellationRequested();
            _runState = RunStateEnum.Started;

            while (!TaskComplete && !_cancellationToken.IsCancellationRequested)
            {
                try
                {
                    while (!_queue.IsEmpty && !_cancellationToken.IsCancellationRequested)
                    {
                        if (_queue.TryDequeue(out T t))
                        {
                            Process(t);
                        }
                        else
                        {
                            _logger.Debug("Dequeue failed");
                        }
                    }
                    if (!_cancellationToken.IsCancellationRequested)
                    {
                        if (_timeoutMs > 0)
                        {
                            if (!_resetEvent.Wait(_timeoutMs, _cancellationToken))
                            {
                                // timeout, we consider this an error an exit
                                _logger.DebugFormat("Wait timed out after {0} ms - exiting", _timeoutMs);
                                TaskComplete = true;
                            }
                        }
                        else
                        {
                            _resetEvent.Wait(_cancellationToken);
                        }

                        _resetEvent.Reset();
                    }
                }
                catch (AggregateException ex)
                {
                    if (ex.Flatten().InnerExceptions.Any(x => x is TaskCanceledException))
                    {
                        _logger.Debug("Task cancelled");
                    }
                    else
                    {
                        _logger.Error("Error in Process - ", ex);
                        TaskComplete = true;
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.Debug("Task cancelled");
                    TaskComplete = true;
                }
                catch (Exception ex)
                {
                    _logger.Error("Error in Process - ", ex);
                    TaskComplete = true;     // unhandled exceptions generally bad news, we want this to stop whatever it is doing
                }
            }

            OnStop();

            _runState = RunStateEnum.Stopped;

            _logger.DebugFormat("ThreadProc - exiting TaskComplete({0})", TaskComplete);
        }

        public void Stop()
        {
            if (_taskList.Count == 0)
            {
                return;
            }

            try
            {
                _ctSource.Cancel();
                foreach (var task in _taskList)
                {
                    task.Wait();
                }
            }
            catch (Exception ex)
            {
                _logger.Debug("Stop", ex);
                throw;
            }
        }

        protected abstract bool OnStart();
        protected abstract bool OnStop();
        protected abstract void Process(T t);
        protected abstract void OnItemQueued(T t);
    }
}
