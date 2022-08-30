namespace Linn.Purchasing.Scheduling.Host.Jobs
{
    using Linn.Common.Messaging.RabbitMQ.Dispatchers;
    using Linn.Common.Persistence;
    using Linn.Common.Scheduling.Triggers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources.Messages;

    public class SupplierAutoEmailsScheduler : BackgroundService
    {
        private readonly IMessageDispatcher<EmailOrderBookMessageResource> emailOrderBookMessageDispatcher;

        private readonly IMessageDispatcher<EmailMonthlyForecastReportMessageResource> emailMonthlyForecastMessageDispatcher;

        private readonly IServiceProvider serviceProvider;

        public SupplierAutoEmailsScheduler(
            IMessageDispatcher<EmailOrderBookMessageResource> emailOrderBookMessageDispatcher,
            IMessageDispatcher<EmailMonthlyForecastReportMessageResource> emailMonthlyForecastMessageDispatcher,
            IServiceProvider serviceProvider)
        {
            this.emailOrderBookMessageDispatcher = emailOrderBookMessageDispatcher;
            this.emailMonthlyForecastMessageDispatcher = emailMonthlyForecastMessageDispatcher;
            this.serviceProvider = serviceProvider;
        }

        // Dispatches a message to instruct various reports to be emailed to suppliers on Monday mornings 6am
        // emails configured here https://app.linn.co.uk/purch/planning/plautoem.aspx
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            // every day at 6am
            var trigger = new DailyTrigger(6);

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
                            this.emailOrderBookMessageDispatcher.Dispatch(new EmailOrderBookMessageResource
                                                                              {
                                                                                  ForSupplier = s.SupplierId,
                                                                                  Timestamp = DateTime.Now,
                                                                                  ToAddress = s.EmailAddress,
                                                                                  Test = true // todo - set false to send for real!
                                                                              });
                        }

                        // dispatch a message for all the suppliers to receive a Monthly forecast
                        foreach (var s in repository.FilterBy(x => x.Forecast.Equals("REPORT")
                                 && x.ForecastInterval.Equals("Weekly")))
                        {
                            this.emailMonthlyForecastMessageDispatcher
                                .Dispatch(new EmailMonthlyForecastReportMessageResource
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
