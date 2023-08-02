namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    using System;
    using System.Collections.Generic;
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

        public void SendDeliveryReminder(IEnumerable<PurchaseOrderDeliveryKey> deliveryKeys, bool test = false)
        {
            var deliveries = deliveryKeys.Select(k => this.repository.FindById(k)).ToList();
            var csvData = new List<List<string>>();
            var headers = new List<string> { "Order Number", "Linn Part Number", "Item", "Delivery Qty", "Promise Date", "Courier", "Tracking Number / Link", "Additional Comments" };
            csvData.Add(headers);

            foreach (var d in deliveries)
            {
                csvData.Add(new List<string>
                                {
                                    d.PurchaseOrderDetail.OrderNumber.ToString(),
                                    d.PurchaseOrderDetail.PartNumber, 
                                    d.PurchaseOrderDetail.SuppliersDesignation, 
                                    d.OrderDeliveryQty.ToString(), 
                                    d.DateAdvised.GetValueOrDefault().ToString("dd-MM-yyyy")
                                });
            }

            var supplier = deliveries.First().PurchaseOrderDetail.PurchaseOrder.Supplier;

            var toAddress = test 
                                ? ConfigurationManager.Configuration["ORDER_BOOK_TEST_ADDRESS"] 
                                : supplier.SupplierContacts.First(x => x.IsMainOrderContact == "Y").EmailAddress;

            var vendorManagerAddress = supplier.VendorManager.Employee.PhoneListEntry.EmailAddress;

            var vendorManagerName = supplier.VendorManager.Employee.FullName;

            var body = $"Linn sent you Purchase Order(s) that you have previously confirmed.";
            body += Environment.NewLine;
            body += Environment.NewLine;

            body +=
                "Please see attached spreadsheet for orders that are now due for delivery. "
                + "Can you please complete the spreadsheet with the shipping details and tracking number for each delivery and return to Linn.";
            body += Environment.NewLine;
            body += Environment.NewLine;

            body += "Kind Regards,";
            body += Environment.NewLine;
            body += vendorManagerName;
            body += Environment.NewLine;
            body += "Linn Products Ltd";

            this.emailService.SendEmail(
                toAddress.Trim(), 
                supplier.Name, 
                null, 
                null, 
                vendorManagerAddress, 
                vendorManagerName,  
                "LINN PRODUCTS PURCHASE ORDER DELIVERY REMINDER",
                body,
                new List<Attachment>
                    {
                        new CsvAttachment(null, csvData, "Deliveries")
                    });
            
            foreach (var d in deliveries)
            {
                d.ReminderSent = "Y";
            } 
        }
    }
}
