namespace Linn.Purchasing.Messaging.Handlers
{
    using System;
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Newtonsoft.Json;

    public class EmailPurchaseOrderReminderMessageHandler : Handler<EmailPurchaseOrderReminderMessage>
    {
        public EmailPurchaseOrderReminderMessageHandler(ILog logger)
            : base(logger)
        {
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
                        $"Sending Purchase Order Reminder for Order/Line/Deliver: {resource.OrderNumber}/{resource.OrderLine}/{resource.DeliverySeq}");
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
