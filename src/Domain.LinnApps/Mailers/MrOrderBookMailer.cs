namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    using System;
    using System.IO;

    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Serialization;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class MrOrderBookMailer : IMrOrderBookMailer
    {
        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IEmailService emailService;

        private readonly IMrOrderBookReportService reportService;
         
       private readonly ISingleRecordRepository<TqmsMaster> tqmsMaster;

        public MrOrderBookMailer(
            IRepository<Supplier, int> supplierRepository,
            IMrOrderBookReportService reportService,
            IEmailService emailService,
            ISingleRecordRepository<TqmsMaster> tqmsMaster)
        {
            this.emailService = emailService;
            this.reportService = reportService;
            this.supplierRepository = supplierRepository;
            this.tqmsMaster = tqmsMaster;
        }

        public void SendOrderBookEmail(string toAddress, int toSupplier, string timestamp, bool test = false)
        {
            var supplier = this.supplierRepository.FindById(toSupplier);

            if (string.IsNullOrEmpty(toAddress))
            {
                // todo - use suppliers main order contact email address
                throw new MrOrderBookEmailException($"No recipient address set for: {toSupplier}");
            }

            var vendorManagerAddress = supplier.VendorManager.Employee.PhoneListEntry.EmailAddress;
            var vendorManagerName = supplier.VendorManager.Employee.FullName;

            var lastTqmsDate = this.tqmsMaster.GetRecord().DateLastDoTqmsSums;

            // notify the vendor managers if tqms jobs failed
            if (lastTqmsDate != DateTime.Today.Date)
            {
                this.emailService.SendEmail(
                    vendorManagerAddress, 
                    vendorManagerName, 
                    null, 
                    null, 
                    ConfigurationManager.Configuration["PURCHASING_FROM_ADDRESS"],
                    "Purchasing Outgoing",
                    "MR ORDER BOOK EMAIL ERROR",
                    "The MR Order book emails could not be sent because the TQMS jobs did not run over the weekend.",
                    null,
                    null,
                    null);
                return; // don't send the emails to suppliers since the figures won't be up to date
            }

            var export = this.reportService.GetOrderBookExport(toSupplier);
            

            // todo ? - I don't feel great about doing this in the domain.
            // Serializing, converting to csv list etc. should probably be done in the email service
            var stream = new MemoryStream();
            var csvStreamWriter = new CsvStreamWriter(stream);
            csvStreamWriter.WriteModel(export.ConvertToCsvList());
            stream.Position = 0;

            this.emailService.SendEmail(
                test ? ConfigurationManager.Configuration["ORDER_BOOK_TEST_ADDRESS"] : toAddress,
                supplier.Name,
                null,
                null,
                vendorManagerAddress,
                vendorManagerName,
                $"MR Order Book - {timestamp}",
                "Please find Order Book attached",
                "csv",
                stream,
                $"{toSupplier}_linn_order_book_{timestamp}");
        }
    }
}
