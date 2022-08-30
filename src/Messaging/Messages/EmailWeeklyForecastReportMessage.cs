namespace Linn.Purchasing.Messaging.Messages
{
    using Linn.Common.Messaging.RabbitMQ.Messages;

    using RabbitMQ.Client.Events;

    public class EmailMonthlyForecastReportMessage : RabbitMessage
    {
        public const string RoutingKey = "email-monthly-forecast-report";

        public EmailMonthlyForecastReportMessage(BasicDeliverEventArgs e)
            : base(e)
        {
        }
    }
}
