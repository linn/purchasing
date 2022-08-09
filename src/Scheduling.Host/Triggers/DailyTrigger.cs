namespace Linn.Purchasing.Scheduling.Host.Triggers // todo - move to common
{
    public class DailyTrigger : IDisposable
    {
        public DailyTrigger(int hour, int minute = 0, int second = 0)
        {
            this.TriggerHour = new TimeSpan(hour, minute, second);
            this.CancellationToken = new CancellationTokenSource();
            this.RunningTask = Task.Run(
                async () =>
                    {
                        while (true)
                        {
                            var triggerTime = DateTime.Today + this.TriggerHour - DateTime.Now;
                            if (triggerTime < TimeSpan.Zero)
                            {
                                triggerTime = triggerTime.Add(new TimeSpan(24, 0, 0));
                            }

                            await Task.Delay(triggerTime, this.CancellationToken.Token);

                            this.OnTimeTriggered?.Invoke();
                        }
                    },
                this.CancellationToken.Token);
        }

        ~DailyTrigger() => this.Dispose();

        public event Action? OnTimeTriggered;

        public TimeSpan TriggerHour { get; }

        public CancellationTokenSource? CancellationToken { get; set; }

        public Task? RunningTask { get; set; }

        public void Dispose()
        {
            this.CancellationToken?.Cancel();
            this.CancellationToken?.Dispose();
            this.CancellationToken = null;
            this.RunningTask?.Dispose();
            this.RunningTask = null;
        }
    }
}
