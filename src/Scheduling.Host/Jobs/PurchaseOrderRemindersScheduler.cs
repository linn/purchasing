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
            this.log.Info("Supplier Auto Emails Scheduler Running...");

          
            var dailyTrigger = new DailyTrigger(this.currentTime, 8);

            dailyTrigger.OnTimeTriggered += () =>
                {
                    using IServiceScope scope = this.serviceProvider.CreateScope();

                    IPurchaseOrderDeliveryRepository deliveryRepository =
                        scope.ServiceProvider.GetRequiredService<IPurchaseOrderDeliveryRepository>();

                    var inDateRange = deliveryRepository.FindAll().Where(
                        x => (x.DateAdvised ?? DateTime.MinValue).Date - this.currentTime().Date - TimeSpan.FromDays(2)
                             == TimeSpan.Zero);

                    var deliveries = inDateRange.Where(
                        x => x.PurchaseOrderDetail.PurchaseOrder.OrderMethodName == "MANUAL"
                             && x.ReminderSent != "Y"
                             && x.PurchaseOrderDetail.PurchaseOrder.Supplier.ReceivesOrderReminders == "Y"
                             && x.DateAdvised.HasValue && x.QuantityOutstanding.GetValueOrDefault() > 0).ToList();

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
                };

            await Task.Delay(1, stoppingToken);
        }
    }
}
