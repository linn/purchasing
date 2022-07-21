namespace Linn.Purchasing.Scheduling.Host.Jobs
{
    using Linn.Purchasing.Domain.LinnApps.Dispatchers;
    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Triggers;

    public class EmailOrderBookScheduler : BackgroundService
    {
        private readonly IMessageDispatcher<EmailOrderBookMessageResource> dispatcher;

        public EmailOrderBookScheduler(IMessageDispatcher<EmailOrderBookMessageResource> dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var trigger = new DailyTrigger(14, 00); // every day at 5:00pm

            // do the following
            trigger.OnTimeTriggered += () =>
                {
                    // check if its sunday
                    // if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    // {
                        // dispatch a message if it is
                    this.dispatcher.Dispatch(new EmailOrderBookMessageResource { SupplierId = 38577 });
                    //}
                };

            return Task.Delay(1, stoppingToken);
        }
    }
}
