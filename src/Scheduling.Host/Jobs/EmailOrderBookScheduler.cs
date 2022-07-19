namespace Linn.Purchasing.Scheduling.Host.Jobs
{
    using Linn.Purchasing.Domain.LinnApps.Dispatchers;
    using Linn.Purchasing.Resources.Messages;

    public class EmailOrderBookScheduler : BackgroundService, IDisposable
    {
        private readonly IMessageDispatcher<EmailOrderBookMessageResource> dispatcher;

        private Timer? timer;

        public EmailOrderBookScheduler(IMessageDispatcher<EmailOrderBookMessageResource> dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            this.timer?.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.timer = new Timer(
                this.SendMessage,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(300)); // send a message every 5 minutes

            return Task.CompletedTask;
        }

        private void SendMessage(object? state)
        {
            this.dispatcher.Dispatch(new EmailOrderBookMessageResource());
        }
    }
}
