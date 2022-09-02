namespace Linn.Purchasing.Scheduling.Host.Triggers
{
    public class WeeklyTrigger : IDisposable
    {
        public WeeklyTrigger(CurrentTime currentTime, DayOfWeek day, int hour, int minute = 0, int second = 0)
        {
            this.TriggerHour = new TimeSpan((int)day, hour, minute, second);
            this.CancellationToken = new CancellationTokenSource();
            this.RunningTask = Task.Run(
                async () =>
                    {
                        while (true)
                        {
                            var startOfWeek = currentTime().AddDays(-(int)currentTime().DayOfWeek).Date; // Sunday, 00:00

                            var triggerTime = startOfWeek + this.TriggerHour - currentTime();

                            if (triggerTime < TimeSpan.Zero)
                            {
                                triggerTime = triggerTime.Add(new TimeSpan(24 * 7, 0, 0));
                            }

                            await Task.Delay(triggerTime, this.CancellationToken.Token);

                            this.OnTimeTriggered?.Invoke();
                        }
                    },
                this.CancellationToken.Token);
        }

        ~WeeklyTrigger() => this.Dispose();

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
