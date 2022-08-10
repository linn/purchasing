namespace Linn.Purchasing.Messaging.Handlers
{
    using System;
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Purchasing.Domain.LinnApps.Mailers;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Newtonsoft.Json;

    public class EmailWeeklyForecastReportMessageHandler : Handler<EmailWeeklyForecastReportMessage>
    {
        private readonly ISupplierAutoEmailsMailer mailer;

        public EmailWeeklyForecastReportMessageHandler(
            ILog logger,
            ISupplierAutoEmailsMailer mailer)
            : base(logger)
        {
            this.mailer = mailer;
        }

        public override bool Handle(EmailWeeklyForecastReportMessage message)
        {
            this.Logger.Info("Message received: " + message.Event.RoutingKey);

            try
            {
                var body = message.Event.Body.ToArray();
                var enc = Encoding.UTF8.GetString(body);
                var resource = JsonConvert.DeserializeObject<EmailOrderBookMessageResource>(enc);
                this.Logger.Info("Sending Weekly Forecast email to: " + resource.ForSupplier);

                this.mailer.SendWeeklyForecastEmail(
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
