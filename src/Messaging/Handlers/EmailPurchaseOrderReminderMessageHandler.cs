namespace Linn.Purchasing.Messaging.Handlers
{
    using System;
    using System.Linq;
    using System.Text;

    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Mailers;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    public class EmailPurchaseOrderReminderMessageHandler : Handler<EmailPurchaseOrderReminderMessage>
    {
        private readonly IServiceProvider serviceProvider;

        private readonly ITransactionManager transactionManager;

        public EmailPurchaseOrderReminderMessageHandler(
            ILog logger,
            IServiceProvider serviceProvider,
            ITransactionManager transactionManager)
            : base(logger)
        {
            this.serviceProvider = serviceProvider;
            this.transactionManager = transactionManager;
        }

        public override bool Handle(EmailPurchaseOrderReminderMessage message)
        {
            this.Logger.Info("Message received: " + message.Event.RoutingKey);

            using var scope = this.serviceProvider.CreateScope();

            var mailer = scope.ServiceProvider.GetRequiredService<IPurchaseOrderRemindersMailer>();

            try
            {
                var body = message.Event.Body.ToArray();
                var enc = Encoding.UTF8.GetString(body);
                var resource = JsonConvert.DeserializeObject<EmailPurchaseOrderReminderMessageResource>(enc);
                mailer.SendDeliveryReminder(
                    resource.Deliveries, resource.Test.GetValueOrDefault());
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
