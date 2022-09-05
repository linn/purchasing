﻿namespace Linn.Purchasing.Scheduling.Host.Jobs
{
    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Dispatchers;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources.Messages;
    using Linn.Purchasing.Scheduling.Host.Triggers;

    public class SupplierAutoEmailsScheduler : BackgroundService
    {
        private readonly IMessageDispatcher<EmailOrderBookMessageResource> emailOrderBookMessageDispatcher;

        private readonly IMessageDispatcher<EmailMonthlyForecastReportMessageResource> emailMonthlyForecastMessageDispatcher;

        private readonly IServiceProvider serviceProvider;

        private readonly CurrentTime currentTime;

        private readonly ILog log;

        public SupplierAutoEmailsScheduler(
            IMessageDispatcher<EmailOrderBookMessageResource> emailOrderBookMessageDispatcher,
            IMessageDispatcher<EmailMonthlyForecastReportMessageResource> emailMonthlyForecastMessageDispatcher,
            CurrentTime currentTime,
            ILog log,
            IServiceProvider serviceProvider)
        {
            this.emailOrderBookMessageDispatcher = emailOrderBookMessageDispatcher;
            this.emailMonthlyForecastMessageDispatcher = emailMonthlyForecastMessageDispatcher;
            this.serviceProvider = serviceProvider;
            this.log = log;
            this.currentTime = currentTime;
        }

        // Dispatches a message to instruct various reports to be emailed to suppliers on Monday mornings 6am
        // emails configured here https://app.linn.co.uk/purch/planning/plautoem.aspx
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.log.Info("Supplier Auto Emails Scheduler Running...");
            // every Monday at 6am
            var weeklyTrigger = new WeeklyTrigger(this.currentTime, DayOfWeek.Monday, 6, 0);

            weeklyTrigger.OnTimeTriggered += () => 
                {
                    this.log.Info("Weekly trigger fired.");

                    using IServiceScope scope = this.serviceProvider.CreateScope();

                    IRepository<SupplierAutoEmails, int> repository =
                        scope.ServiceProvider.GetRequiredService<IRepository<SupplierAutoEmails, int>>();

                    // dispatch a message for all the suppliers to receive an order book
                    foreach (var s in repository.FindAll().Where(x => x.OrderBook.Equals("Y")))
                    {
                        this.emailOrderBookMessageDispatcher.Dispatch(
                            new EmailOrderBookMessageResource
                                {
                                    ForSupplier = s.SupplierId,
                                    Timestamp = this.currentTime(),
                                    ToAddress = s.EmailAddress,
                                    Test = true // todo - set false to send for real!
                                });
                    }

                    // dispatch a message for all the suppliers to receive a Monthly forecast
                    foreach (var s in repository.FindAll().Where(x => 
                                 (x.Forecast.Equals("REPORT") 
                                  || x.Forecast.Equals("CSV")) 
                                 && x.ForecastInterval.Equals("WEEKLY")))
                    {
                        this.emailMonthlyForecastMessageDispatcher
                            .Dispatch(new EmailMonthlyForecastReportMessageResource
                                          {
                                              ForSupplier = s.SupplierId,
                                              Timestamp = this.currentTime(),
                                              ToAddress = s.EmailAddress, 
                                              Test = true // todo - set false to send for real!
                                          });
                    }
                };

            // first monday of every month at 6am
            var monthlyTrigger = new MonthlyTrigger(this.currentTime, DayOfWeek.Monday, 6);
            monthlyTrigger.OnTimeTriggered += () =>
            {
                this.log.Info("Monthly trigger fired.");

                using IServiceScope scope = this.serviceProvider.CreateScope();

                IRepository<SupplierAutoEmails, int> repository =
                    scope.ServiceProvider.GetRequiredService<IRepository<SupplierAutoEmails, int>>();

                // dispatch a message for all the suppliers to receive a Monthly forecast
                foreach (var s in repository.FindAll().Where(x =>
                             (x.Forecast.Equals("REPORT")
                              || x.Forecast.Equals("CSV"))
                             && x.ForecastInterval.Equals("MONTHLY")))
                {
                    this.emailMonthlyForecastMessageDispatcher
                        .Dispatch(new EmailMonthlyForecastReportMessageResource
                        {
                            ForSupplier = s.SupplierId,
                            Timestamp = this.currentTime(),
                            ToAddress = s.EmailAddress,
                            Test = true // todo - set false to send for real!
                            });
                }
            };

            await Task.Delay(1, stoppingToken);
        }
    }
}
