namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    using System;
    using System.IO;
    using System.Linq;

    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Serialization;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class SupplierAutoEmailsMailer : ISupplierAutoEmailsMailer
    {
        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IEmailService emailService;

        private readonly IMrOrderBookReportService orderBookReportService;
         
       private readonly ISingleRecordRepository<TqmsMaster> tqmsMaster;

       private readonly IForecastOrdersReportService forecastOrdersReportService;

        public SupplierAutoEmailsMailer(
            IRepository<Supplier, int> supplierRepository,
            IMrOrderBookReportService orderBookReportService,
            IEmailService emailService,
            ISingleRecordRepository<TqmsMaster> tqmsMaster,
            IForecastOrdersReportService forecastOrdersReportService)
        {
            this.emailService = emailService;
            this.orderBookReportService = orderBookReportService;
            this.supplierRepository = supplierRepository;
            this.tqmsMaster = tqmsMaster;
            this.forecastOrdersReportService = forecastOrdersReportService;
        }

        public void SendOrderBookEmail(string toAddress, int toSupplier, string timestamp, bool test = false)
        {
            var supplier = this.supplierRepository.FindById(toSupplier);

            var emailAddress = string.IsNullOrEmpty(toAddress) ? supplier.SupplierContacts
                ?.First(c => c.IsMainOrderContact.Equals("Y"))?.EmailAddress : toAddress;

            this.CheckEmailDetailsOk(emailAddress, supplier);
            
            var vendorManagerAddress = supplier.VendorManager.Employee.PhoneListEntry.EmailAddress;
            var vendorManagerName = supplier.VendorManager.Employee.FullName;

            var export = this.orderBookReportService.GetOrderBookExport(toSupplier);

            // todo ? - I don't feel great about doing this in the domain.
            // Serializing, converting to csv list etc. should probably be done in the email service
            var stream = new MemoryStream();
            var csvStreamWriter = new CsvStreamWriter(stream);
            csvStreamWriter.WriteModel(export.ConvertToCsvList());
            stream.Position = 0;

            this.emailService.SendEmail(
                test ? ConfigurationManager.Configuration["ORDER_BOOK_TEST_ADDRESS"] : emailAddress,
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

        public void SendWeeklyForecastEmail(string toAddress, int toSupplier, string timestamp, bool test = false)
        {
            var supplier = this.supplierRepository.FindById(toSupplier);

            var emailAddress = string.IsNullOrEmpty(toAddress) ? supplier.SupplierContacts
                                   ?.First(c => c.IsMainOrderContact.Equals("Y"))?.EmailAddress : toAddress;

            this.CheckEmailDetailsOk(emailAddress, supplier);

            var vendorManagerAddress = supplier.VendorManager.Employee.PhoneListEntry.EmailAddress;
            var vendorManagerName = supplier.VendorManager.Employee.FullName;

            var export = this.forecastOrdersReportService.GetWeeklyExport(toSupplier);

            var stream = new MemoryStream();
            var csvStreamWriter = new CsvStreamWriter(stream);
            csvStreamWriter.WriteModel(export.ConvertToCsvList());
            stream.Position = 0;

            this.emailService.SendEmail(
                test ? ConfigurationManager.Configuration["ORDER_BOOK_TEST_ADDRESS"] : emailAddress,
                supplier.Name,
                null,
                null,
                vendorManagerAddress,
                vendorManagerName,
                $"Weekly Forecast - {timestamp}",
                "Please find weekly order forecast attached",
                "csv",
                stream,
                $"{toSupplier}_weekly_forecast_{timestamp}");
        }

        private void CheckEmailDetailsOk(string toAddress, Supplier supplier)
        {
            if (string.IsNullOrEmpty(toAddress))
            {
                throw new MrOrderBookEmailException($"No recipient address set for: {supplier.Name}");
            }

            var lastTqmsDate = this.tqmsMaster.GetRecord().DateLastDoTqmsSums;

            // notify the vendor managers if tqms jobs failed
            if (lastTqmsDate != DateTime.Today.Date)
            {
                var msg = "The MR Order book emails could not be sent because the TQMS jobs did not run over the weekend.";
                this.emailService.SendEmail(
                    supplier.VendorManager.Employee.PhoneListEntry.EmailAddress,
                    supplier.VendorManager.Employee.FullName,
                    null,
                    null,
                    ConfigurationManager.Configuration["PURCHASING_FROM_ADDRESS"],
                    "Purchasing Outgoing",
                    "MR ORDER BOOK EMAIL ERROR",
                    msg,
                    null,
                    null,
                    null);

                throw new MrOrderBookEmailException(msg);
            }
        }
    }
}
