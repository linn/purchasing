namespace Linn.Purchasing.Messaging.Handlers
{
    using System;
    using System.Linq;
    using System.Text;

    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Common.Logging;
    using Linn.Common.Messaging.RabbitMQ.Handlers;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Mailers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    public class EmailPurchaseOrderReminderMessageHandler : Handler<EmailPurchaseOrderReminderMessage>
    {

        private readonly IServiceProvider serviceProvider;

        public EmailPurchaseOrderReminderMessageHandler(
            ILog logger,
            IServiceProvider serviceProvider)
            : base(logger)
        {
            this.serviceProvider = serviceProvider;
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
                        $"Sending Purchase Order Reminder for Order/Line/Delivery: " 
                        + $"{resource.OrderNumber}/{resource.OrderLine}/{resource.DeliverySeq}");
                this.SendDeliveryReminder(
                    resource.OrderNumber, resource.OrderLine, resource.DeliverySeq, resource.Test.GetValueOrDefault());
                return true;
            }
            catch (Exception e)
            {
                this.Logger.Error(e.Message, e);
                return false;
            }
        }

        private void SendDeliveryReminder(int orderNumber, int orderLine, int deliverySeq, bool test = false)
        {
            using IServiceScope scope = this.serviceProvider.CreateScope();

            var repository = scope.ServiceProvider.GetRequiredService<IPurchaseOrderDeliveryRepository>();
            var emailService = scope.ServiceProvider.GetService<IEmailService>();
            var transactionManager = scope.ServiceProvider.GetRequiredService<ITransactionManager>();
            var delivery = repository.FindById(
                new PurchaseOrderDeliveryKey
                {
                    OrderNumber = orderNumber,
                    OrderLine = orderLine,
                    DeliverySequence = deliverySeq
                });
            var supplier = delivery.PurchaseOrderDetail.PurchaseOrder.Supplier;

            var toAddress = test
                                ? ConfigurationManager.Configuration["ORDER_BOOK_TEST_ADDRESS"]
                                : supplier.SupplierContacts.First(x => x.IsMainOrderContact == "Y").EmailAddress;

            var vendorManagerAddress = supplier.VendorManager.Employee.PhoneListEntry.EmailAddress;

            var vendorManagerName = supplier.VendorManager.Employee.FullName;

            var body = $"Linn sent you a Purchase Order ${orderNumber} that you have previously confirmed.";
            body += Environment.NewLine;
            body += Environment.NewLine;

            body +=
                "The order is now due for delivery, can you please provide the shipping details including the tracking number.";
            body += Environment.NewLine;
            body += Environment.NewLine;

            body += $"Item: {delivery.PurchaseOrderDetail.SuppliersDesignation}";
            body += Environment.NewLine;
            body += $"Linn Part Number: {delivery.PurchaseOrderDetail.PartNumber}";
            body += Environment.NewLine;
            body += $"Delivery Qty: {delivery.OrderDeliveryQty}";
            body += Environment.NewLine;
            body += $"Promise Date: {delivery.DateAdvised.GetValueOrDefault():dd-MM-yyyy}";
            body += Environment.NewLine;
            body += Environment.NewLine;
            body += "Kind Regards,";
            body += Environment.NewLine;
            body += vendorManagerName;
            body += Environment.NewLine;
            body += "Linn Products Ltd";

            emailService.SendEmail(
                toAddress,
                supplier.Name,
                null,
                null,
                vendorManagerAddress,
                vendorManagerName,
                "LINN PRODUCTS PURCHASE ORDER DELIVERY REMINDER",
                body,
                null);

            delivery.ReminderSent = "Y";

            transactionManager.Commit();
        }
    }
}
