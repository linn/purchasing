namespace Linn.Purchasing.Messaging.Messages
{
    using Linn.Common.Messaging.RabbitMQ.Messages;

    using RabbitMQ.Client.Events;

    public class EmailWeeklyForecastReportMessage : RabbitMessage
    {
        public const string RoutingKey = "email-weekly-forecast-report";

        public EmailWeeklyForecastReportMessage(BasicDeliverEventArgs e)
            : base(e)
        {
        }
    }
}
