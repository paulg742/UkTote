using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace UkTote
{
    public abstract class CancellableTask
    {
        readonly ILog _logger = LogManager.GetLogger(typeof(CancellableTask));

        public enum RunStateEnum
        {
            NotStarted,
            Starting,
            Started,
            Stopping,
            Stopped
        }

        Task _task;
        int _taskThreadId;
        CancellationTokenSource _ctSource;
        protected CancellationToken CancellationToken;
        RunStateEnum _runState = RunStateEnum.NotStarted;
        volatile bool _taskComplete;

        public Action OnUnexpectedCompletion { get; set; }

        public abstract int SleepMs { get; }

        public bool Completed
        {
            get { return _runState == RunStateEnum.Stopped; }
        }

        protected bool CanStart
        {
            get { return _runState == RunStateEnum.NotStarted || _runState == RunStateEnum.Stopped; }
        }

        public bool TaskComplete 
        {
            get
            {
                return _taskComplete;
            }
            set
            {
                _taskComplete = value;
                _logger.DebugFormat("TaskComplete({0})", value);
            }
        }

        public bool Start()
        {
            _logger.Debug("Starting cancellable task...");
            try
            {
                _ctSource = new CancellationTokenSource();
                CancellationToken = _ctSource.Token;
                _runState = RunStateEnum.Starting;
                _task = Task.Factory.StartNew(ThreadProc, CancellationToken);
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
            CancellationToken.ThrowIfCancellationRequested();
            if (OnStart())
            {
                _runState = RunStateEnum.Started;
                _taskThreadId = Thread.CurrentThread.ManagedThreadId;
                _logger.DebugFormat("ThreadProc started ThreadId({0})", _taskThreadId);

                while (!TaskComplete && !CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        DoWork();
                        Task.Delay(SleepMs, CancellationToken).Wait();
                    }
                    catch (AggregateException ex)
                    {
                        if (ex.Flatten().InnerExceptions.Any(x => x is TaskCanceledException))
                        {
                            _logger.Debug("Task cancelled");
                        }
                        else
                        {
                            _logger.Error("Error in DoWork - ", ex);
                            TaskComplete = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Error in DoWork - ", ex);
                        TaskComplete = true;     // unhandled exceptions generally bad news, we want this to stop whatever it is doing
                    }
                }
            }
            OnStop();
            TaskComplete = true;
            if (_runState != RunStateEnum.Stopping)
            {
                _logger.DebugFormat("Task unexpectedly stopping");
                OnUnexpectedCompletion?.Invoke();
            }
            _runState = RunStateEnum.Stopped;
            _logger.DebugFormat("ThreadProc - exiting TaskComplete({0})", TaskComplete);
        }

        public void Stop()
        {
            if (_task == null)
            {
                _runState = RunStateEnum.Stopped;
                TaskComplete = true;
                return;
            }
            _runState = RunStateEnum.Stopping;
            // ensure we're not trying to stop from the thread we're running on
            if (Thread.CurrentThread.ManagedThreadId == _taskThreadId)
            {
                throw new Exception("Task must be stopped from a different thread");
            }
            _logger.Debug("Stopping cancellable task");
            try
            {
                _ctSource.Cancel();
                _task.Wait(5000);
            }
            catch (Exception ex)
            {
                _logger.Debug("Stop", ex);
                throw;
            }
            finally
            {
                _logger.Debug("Cancellable task stopped");
                _task = null;
            }
        }

        protected abstract bool OnStart();
        protected abstract bool OnStop();
        protected abstract void DoWork();
    }
}
