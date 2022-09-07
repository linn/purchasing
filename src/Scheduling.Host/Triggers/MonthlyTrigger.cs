namespace Linn.Purchasing.Scheduling.Host.Triggers
{
    public class MonthlyTrigger : IDisposable
    {
        public MonthlyTrigger(CurrentTime currentTime, DayOfWeek day, int hour, int minute = 0, int second = 0)
        {
            int dayOfMonth = 0;

            foreach (DateTime date in AllDatesInMonth(currentTime().Year, currentTime().Month))
            {
                if (date.DayOfWeek == day)
                {
                    dayOfMonth = date.Day;
                    break;
                }
            }
                
            this.TriggerHour = new TimeSpan(((dayOfMonth - 1) * 24) + hour, minute, second);
            this.CancellationToken = new CancellationTokenSource();
            
            this.RunningTask = Task.Run(
                async () =>
                    {
                        while (true)
                        {
                            var startOfMonth = new DateTime(currentTime().Year, currentTime().Month, 1);

                            var triggerTime = startOfMonth + this.TriggerHour - currentTime();

                            if (triggerTime < TimeSpan.Zero)
                            {
                                triggerTime = triggerTime
                                    .Add(new TimeSpan(24 * DateTime.DaysInMonth(currentTime().Year, currentTime().Month), 0, 0));
                            }

                            await Task.Delay(triggerTime, this.CancellationToken.Token);

                            this.OnTimeTriggered?.Invoke();
                        }
                    },
                this.CancellationToken.Token);
        }

        ~MonthlyTrigger() => this.Dispose();

        public event Action? OnTimeTriggered;

        public TimeSpan TriggerHour { get; }

        public CancellationTokenSource? CancellationToken { get; set; }

        public Task? RunningTask { get; set; }

        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                yield return new DateTime(year, month, day);
            }
        }

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
