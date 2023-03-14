namespace Linn.Purchasing.Messaging.Handlers
{
    using System;
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Mailers;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Newtonsoft.Json;

    public class EmailPurchaseOrderReminderMessageHandler : Handler<EmailPurchaseOrderReminderMessage>
    {
        private readonly IPurchaseOrderRemindersMailer mailer;

        private readonly ITransactionManager transactionManager;

        public EmailPurchaseOrderReminderMessageHandler(
            ILog logger,
            IPurchaseOrderRemindersMailer mailer,
            ITransactionManager transactionManager)
            : base(logger)
        {
            this.mailer = mailer;
            this.transactionManager = transactionManager;
        }

        public override bool Handle(EmailPurchaseOrderReminderMessage message)
        {
            this.Logger.Info("Message received: " + message.Event.RoutingKey);
            try
            {
                var body = message.Event.Body.ToArray();
                var enc = Encoding.UTF8.GetString(body);
                var resource = JsonConvert.DeserializeObject<EmailPurchaseOrderReminderMessageResource>(enc);
                this.Logger.Info(
                        $"Sending Purchase Order Reminder for Order/Line/Delivery: " 
                        + $"{resource.OrderNumber}/{resource.OrderLine}/{resource.DeliverySeq}");
                this.mailer.SendDeliveryReminder(
                    resource.OrderNumber, resource.OrderLine, resource.DeliverySeq, resource.Test.GetValueOrDefault());
                this.transactionManager.Commit();
                return true;
            }
            catch (Exception e)
            {
                this.Logger.Error(e.Message, e);
                return false;
            }
        }
    }
}
