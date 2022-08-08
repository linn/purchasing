namespace Linn.Purchasing.Scheduling.Host.Jobs
{
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Dispatchers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Triggers;

    public class EmailOrderBookScheduler : BackgroundService
    {
        private readonly IMessageDispatcher<EmailOrderBookMessageResource> dispatcher;

        private readonly IServiceProvider serviceProvider;

        public EmailOrderBookScheduler(
            IMessageDispatcher<EmailOrderBookMessageResource> dispatcher,
            IServiceProvider serviceProvider)
        {
            this.dispatcher = dispatcher;
            this.serviceProvider = serviceProvider;
        }

        // Dispatches a message to instruct Order Books to be emailed to suppliers on Monday mornings at 230
        // emails configured here https://app.linn.co.uk/purch/planning/plautoem.aspx
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            // every day at 230am
            var trigger = new DailyTrigger(2, 30);

            // do the following
            trigger.OnTimeTriggered += () =>
                {
                    // check if its monday
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                    {
                        using IServiceScope scope = this.serviceProvider.CreateScope();

                        IRepository<SupplierAutoEmails, int> repository =
                            scope.ServiceProvider.GetRequiredService<IRepository<SupplierAutoEmails, int>>();

                        // dispatch a message for all the suppliers to receive an order book
                        foreach (var s in repository.FilterBy(x => x.OrderBook.Equals("Y")))
                        {
                            this.dispatcher.Dispatch(new EmailOrderBookMessageResource
                                                         {
                                                             ForSupplier = s.SupplierId,
                                                             Timestamp = DateTime.Now,
                                                             ToAddress = s.EmailAddress,
                                                             Test = true // todo - set false to send for real!
                                                         });
                        }
                    }
                };

            await Task.Delay(1, stoppingToken);
        }
    }
}
