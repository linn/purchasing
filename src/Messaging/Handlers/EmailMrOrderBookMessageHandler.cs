namespace Linn.Purchasing.Messaging.Handlers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Linn.Common.Email;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Serialization;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Messaging.Messages;
    using Linn.Purchasing.Resources.Messages;

    using Newtonsoft.Json;

    public class EmailMrOrderBookMessageHandler : Handler<EmailMrOrderBookMessage>
    {
        private readonly IMrOrderBookReportService reportService;

        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IEmailService emailService;

        public EmailMrOrderBookMessageHandler(
            ILog logger, 
            IMrOrderBookReportService reportService,
            IEmailService emailService,
            IRepository<Supplier, int> supplierRepository)
            : base(logger)
        {
            this.emailService = emailService;
            this.reportService = reportService;
            this.supplierRepository = supplierRepository;
        }

        public override bool Handle(EmailMrOrderBookMessage message)
        {
           this.Logger.Info("Message received: " + message.Event.RoutingKey);

            try
            {
                var body = message.Event.Body.ToArray();
                var enc = Encoding.UTF8.GetString(body);
                var resource = JsonConvert.DeserializeObject<EmailOrderBookMessageResource>(enc);
                var supplier = this.supplierRepository.FindById(resource.SupplierId);
                
                var contact = supplier.SupplierContacts
                    .First(x => x.IsMainOrderContact.Equals("Y"));
                
                if (contact?.EmailAddress == null)
                {
                    this.Logger.Error(
                        $"No main order contact with a valid email address for supplier: {resource.SupplierId}");
                    return false;
                }
                this.Logger.Info("Building Report...");

                var export = this.reportService.GetOrderBookExport(resource.SupplierId);
                
                var stream = new MemoryStream();
                var csvStreamWriter = new CsvStreamWriter(stream);
                csvStreamWriter.WriteModel(export.ConvertToCsvList());
                stream.Position = 0;
                this.Logger.Info("sending email to " + contact.EmailAddress);

                this.emailService.SendEmail(
                    contact.EmailAddress,
                    supplier.Name,
                    null,
                    null,
                    contact.EmailAddress,
                    "Lewis",
                    "MR Order Book",
                    "Please find Order Book attached",
                    "csv",
                    stream,
                    "order-book");
                return true;
            }
            catch (JsonReaderException e)
            {
                this.Logger.Error(e.Message);
                return false;
            }
            catch (Exception e)
            {
                this.Logger.Error(e.Message);
                return false;
            }
        }
    }
}
