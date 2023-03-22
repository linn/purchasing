namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    using System;
    using System.Linq;

    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public class PurchaseOrderRemindersMailer : IPurchaseOrderRemindersMailer
    {
        private readonly IEmailService emailService;

        private readonly IPurchaseOrderDeliveryRepository repository;

        public PurchaseOrderRemindersMailer(
            IEmailService emailService,
            IPurchaseOrderDeliveryRepository repository)
        {
            this.emailService = emailService;
            this.repository = repository;
        }

        public void SendDeliveryReminder(int orderNumber, int orderLine, int deliverySeq, bool test = false)
        {
            var delivery = this.repository.FindById(
                new PurchaseOrderDeliveryKey
                    {
                        OrderNumber = orderNumber, OrderLine = orderLine, DeliverySequence = deliverySeq
                    });
            var supplier = delivery.PurchaseOrderDetail.PurchaseOrder.Supplier;

            var toAddress = test 
                                ? ConfigurationManager.Configuration["ORDER_BOOK_TEST_ADDRESS"] 
                                : supplier.SupplierContacts.First(x => x.IsMainOrderContact == "Y").EmailAddress;

            var vendorManagerAddress = supplier.VendorManager.Employee.PhoneListEntry.EmailAddress;

            var vendorManagerName = supplier.VendorManager.Employee.FullName;

            var body = $"Linn sent you a Purchase Order {delivery.OrderNumber} that you have previously confirmed.";
            body += Environment.NewLine;
            body += Environment.NewLine;

            body +=
                "The order is now due for delivery, can you please provide the shipping details including the tracking number.";
            body += Environment.NewLine;
            body += Environment.NewLine;
            body += Environment.NewLine;

            body += $"Item: {delivery.PurchaseOrderDetail.SuppliersDesignation}";
            body += Environment.NewLine;
            body += Environment.NewLine;

            body += $"Linn Part Number: {delivery.PurchaseOrderDetail.PartNumber}";
            body += Environment.NewLine;
            body += Environment.NewLine;

            body += $"Delivery Qty: {delivery.OrderDeliveryQty}";
            body += Environment.NewLine;
            body += Environment.NewLine;

            body += $"Promise Date: {delivery.DateAdvised.GetValueOrDefault():dd-MM-yyyy}";
            body += Environment.NewLine;
            body += Environment.NewLine;
            body += Environment.NewLine;

            body += "Kind Regards,";
            body += Environment.NewLine;
            body += vendorManagerName;
            body += Environment.NewLine;
            body += "Linn Products Ltd";

            this.emailService.SendEmail(
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
        }
    }
}
