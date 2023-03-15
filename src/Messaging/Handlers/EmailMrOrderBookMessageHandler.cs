namespace Linn.Purchasing.Messaging.Handlers
{
    using System;
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
    using Linn.Purchasing.Domain.LinnApps.Mailers;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    public class EmailMrOrderBookMessageHandler : Handler<EmailMrOrderBookMessage>
    {
        private readonly IServiceProvider serviceProvider;

        public EmailMrOrderBookMessageHandler(
            ILog logger,
            IServiceProvider serviceProvider)
            : base(logger)
        {
            this.serviceProvider = serviceProvider;
        }

        public override bool Handle(EmailMrOrderBookMessage message)
        {
            this.Logger.Info("Message received: " + message.Event.RoutingKey);
            using var scope = this.serviceProvider.CreateScope();

            var mailer = scope.ServiceProvider.GetRequiredService<ISupplierAutoEmailsMailer>();

            try
            {
                var body = message.Event.Body.ToArray();
                var enc = Encoding.UTF8.GetString(body);
                var resource = JsonConvert.DeserializeObject<EmailOrderBookMessageResource>(enc);
                this.Logger.Info("Sending MR order book email to: " + resource.ForSupplier);

                mailer.SendOrderBookEmail(
                    resource.ToAddress,
                    resource.ForSupplier, 
                    resource.Timestamp.ToShortTimeString(), 
                    resource.Test,
                    resource.BypassMrpCheck.GetValueOrDefault());
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
