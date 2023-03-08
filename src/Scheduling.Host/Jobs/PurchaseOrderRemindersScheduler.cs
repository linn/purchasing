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

            using IServiceScope scope = this.serviceProvider.CreateScope();

            IPurchaseOrderDeliveryRepository deliveryRepository =
                scope.ServiceProvider.GetRequiredService<IPurchaseOrderDeliveryRepository>();

            var dailyTrigger = new DailyTrigger(this.currentTime, 8);

            dailyTrigger.OnTimeTriggered += () =>
                {
                    var del = deliveryRepository.FindAll();
                    var deliveries = deliveryRepository.FindAll().Where(
                        x => x.PurchaseOrderDetail.PurchaseOrder.Supplier.ReceivesOrderReminders == "Y" 
                            && x.DateAdvised.HasValue && x.QuantityOutstanding.GetValueOrDefault() > 0
                             &&  x.DateAdvised.GetValueOrDefault().Date - this.currentTime().Date - TimeSpan.FromDays(2) == TimeSpan.Zero);

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
