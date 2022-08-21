namespace UkTote
{
    public class WatchdogTimer : CancellableTask
    {
        public event Action<string>? OnTimeout;

        private readonly ManualResetEventSlim _resetEvent = new ManualResetEventSlim();
        private readonly int _timeoutMs;

        public WatchdogTimer(int timeoutMs)
        {
            _timeoutMs = timeoutMs;
        }

        public override int SleepMs => 0;

        protected override bool OnStart()
        {
            _resetEvent.Reset();
            return true;
        }

        protected override bool OnStop()
        {
            return true;
        }

        protected override void DoWork()
        {
            var index = WaitHandle.WaitAny(new[]
            {
                CancellationToken.WaitHandle,
                _resetEvent.WaitHandle
            }, _timeoutMs);
            
            switch (index)
            {
                case 0:
                    return;

                case 1:
                    _resetEvent.Reset();
                    break;

                case WaitHandle.WaitTimeout:
                    OnTimeout?.Invoke("Watchdog timeout");
                    TaskComplete = true;
                    break;
            }
        }

        public void Dispose()
        {
            Stop();
        }

        public void Kick()
        {
            _resetEvent.Set();
        }
    }
}
