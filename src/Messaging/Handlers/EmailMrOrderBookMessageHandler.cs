namespace Linn.Purchasing.Messaging.Handlers
{
    using System;
    using System.IO;
    using System.Text;

    using Linn.Common.Configuration;
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
                var supplier = this.supplierRepository.FindById(resource.ForSupplier);
                var test = resource.Test ? "TEST " : string.Empty;

                this.Logger.Info($"{test}Order Book Email for Supplier {resource.ForSupplier} at {resource.Timestamp}");

                if (string.IsNullOrEmpty(resource.ToAddress))
                {
                    this.Logger.Error(
                        $"No recipient address set for: {resource.ForSupplier}");
                    return false;
                }

                this.Logger.Info("Building Report...");

                var export = this.reportService.GetOrderBookExport(resource.ForSupplier);
                
                var stream = new MemoryStream();
                var csvStreamWriter = new CsvStreamWriter(stream);
                csvStreamWriter.WriteModel(export.ConvertToCsvList());
                stream.Position = 0;

                this.Logger.Info("sending email to " + resource.ToAddress);

                this.emailService.SendEmail(
                    resource.Test ? ConfigurationManager.Configuration["ORDER_BOOK_TEST_ADDRESS"] : resource.ToAddress,
                    supplier.Name,
                    null,
                    null,
                    ConfigurationManager.Configuration["PURCHASING_OUTGOING_ADDRESS"],
                    "Linn",
                    $"MR Order Book - {resource.Timestamp.ToShortDateString()}",
                    "Please find Order Book attached",
                    "csv",
                    stream,
                    $"linn_order_book_{resource.Timestamp.ToShortDateString()}");
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
