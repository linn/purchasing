namespace Linn.Purchasing.Messaging.Messages // todo - move to Common
{
    using RabbitMQ.Client.Events;

    public class RabbitMessage
    {
        public RabbitMessage(BasicDeliverEventArgs e)
        {
            this.Event = e;
        }

        public BasicDeliverEventArgs Event { get; set; }
    }
}
