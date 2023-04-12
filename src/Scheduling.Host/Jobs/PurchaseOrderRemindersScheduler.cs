namespace Linn.Purchasing.Scheduling.Host.Jobs
{
    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Dispatchers;
    using Linn.Common.Scheduling;
    using Linn.Common.Scheduling.Triggers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources.Messages;

    public class PurchaseOrderRemindersScheduler : BackgroundService
    {
        private readonly IMessageDispatcher<EmailPurchaseOrderReminderMessageResource> dispatcher;

        private readonly IServiceProvider serviceProvider;

        private readonly CurrentTime currentTime;

        private readonly ILog log;

        public PurchaseOrderRemindersScheduler(
            IMessageDispatcher<EmailPurchaseOrderReminderMessageResource> dispatcher,
            CurrentTime currentTime,
            ILog log,
            IServiceProvider serviceProvider)
        {
            this.dispatcher = dispatcher;
            this.currentTime = currentTime;
            this.serviceProvider = serviceProvider;
            this.log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.log.Info("Purchase Order Reminder Emails Scheduler Running...");

            var dailyTrigger = new DailyTrigger(this.currentTime, 11, 55, 0);

            dailyTrigger.OnTimeTriggered += () =>
                {

                    using IServiceScope scope = this.serviceProvider.CreateScope();

                    this.log.Info("Looking Up Reminders to send...");

                    IPurchaseOrderDeliveryRepository deliveryRepository =
                        scope.ServiceProvider.GetRequiredService<IPurchaseOrderDeliveryRepository>();

                    var inDateRange = deliveryRepository.FindAll().Where(
                        x => (x.DateAdvised ?? DateTime.MinValue).Date == this.currentTime().AddDays(2).Date);

                    this.log.Info("Found " + inDateRange.Count() + " in date range.");

                    var deliveries = inDateRange.Where(
                        x => x.PurchaseOrderDetail.PurchaseOrder.OrderMethodName == "MANUAL"
                             && x.ReminderSent != "Y"
                             && x.PurchaseOrderDetail.PurchaseOrder.Supplier.ReceivesOrderReminders == "Y"
                             && x.DateAdvised.HasValue && x.QuantityOutstanding.GetValueOrDefault() > 0).ToList();

                    this.log.Info("Found " + deliveries.Count() + " eligible for a reminder email.");


                    if (deliveries.Count > 0)
                    {
                        this.log.Info($"Sending ${deliveries.Count} Emails: ");


                        foreach (var d in deliveries)
                        {
                            this.dispatcher.Dispatch(new EmailPurchaseOrderReminderMessageResource
                                                         {
                                                             OrderNumber = d.OrderNumber,
                                                             OrderLine = d.OrderLine,
                                                             DeliverySeq = d.DeliverySeq,
                                                             Timestamp = DateTime.Now,
                                                             Test = true
                                                         });
                        }
                    }
                };

            await Task.Delay(1, stoppingToken);
        }
    }
}
