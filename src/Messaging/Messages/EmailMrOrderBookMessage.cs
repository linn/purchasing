namespace Linn.Purchasing.Messaging.Messages
{
    using RabbitMQ.Client.Events;

    public class EmailMrOrderBookMessage : RabbitMessage
    {
        public const string RoutingKey = "email-order-book";

        public EmailMrOrderBookMessage(BasicDeliverEventArgs e)
            : base(e)
        {
        }
    }
}
