namespace Linn.Purchasing.Messaging.Messages
{
    using Linn.Common.Messaging.RabbitMQ.Messages;

    using RabbitMQ.Client.Events;

    public class EmailPurchaseOrderReminderMessage : RabbitMessage
    {
        public const string RoutingKey = "email-purchase-order-reminder";

        public EmailPurchaseOrderReminderMessage(BasicDeliverEventArgs e)
            : base(e)
        {
        }
    }
}
