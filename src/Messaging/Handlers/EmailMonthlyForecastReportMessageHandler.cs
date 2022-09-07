namespace Linn.Purchasing.Messaging.Handlers
{
    using System;
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
    using Linn.Purchasing.Domain.LinnApps.Mailers;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Newtonsoft.Json;

    public class EmailMonthlyForecastReportMessageHandler : Handler<EmailMonthlyForecastReportMessage>
    {
        private readonly ISupplierAutoEmailsMailer mailer;

        public EmailMonthlyForecastReportMessageHandler(
            ILog logger,
            ISupplierAutoEmailsMailer mailer)
            : base(logger)
        {
            this.mailer = mailer;
        }

        public override bool Handle(EmailMonthlyForecastReportMessage message)
        {
            this.Logger.Info("Message received: " + message.Event.RoutingKey);

            try
            {
                var body = message.Event.Body.ToArray();
                var enc = Encoding.UTF8.GetString(body);
                var resource = JsonConvert.DeserializeObject<EmailOrderBookMessageResource>(enc);
                this.Logger.Info("Sending Monthly Forecast email to: " + resource.ForSupplier);

                this.mailer.SendMonthlyForecastEmail(
                    resource.ToAddress, resource.ForSupplier, resource.Timestamp.ToShortTimeString(), resource.Test);
                return true;
            }
            catch (Exception e)
            {
                this.Logger.Error(e.Message);
                return false;
            }
        }
    }
}
