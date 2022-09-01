namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    using System;
    using System.IO;
    using System.Linq;

    using Linn.Common.Configuration;
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class SupplierAutoEmailsMailer : ISupplierAutoEmailsMailer
    {
        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IEmailService emailService;

        private readonly IMrOrderBookReportService orderBookReportService;
         
       private readonly ISingleRecordRepository<MrMaster> mrMaster;

       private readonly IForecastOrdersReportService forecastOrdersReportService;

        public SupplierAutoEmailsMailer(
            IRepository<Supplier, int> supplierRepository,
            IMrOrderBookReportService orderBookReportService,
            IEmailService emailService,
            ISingleRecordRepository<MrMaster> mrMaster,
            IForecastOrdersReportService forecastOrdersReportService)
        {
            this.emailService = emailService;
            this.orderBookReportService = orderBookReportService;
            this.supplierRepository = supplierRepository;
            this.mrMaster = mrMaster;
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
                null,
                $"{toSupplier}_linn_order_book_{timestamp}",
                export);
        }

        public void SendMonthlyForecastEmail(string toAddress, int toSupplier, string timestamp, bool test = false)
        {
            var supplier = this.supplierRepository.FindById(toSupplier);

            var emailAddress = string.IsNullOrEmpty(toAddress) ? supplier.SupplierContacts
                                   ?.First(c => c.IsMainOrderContact.Equals("Y"))?.EmailAddress : toAddress;

            this.CheckEmailDetailsOk(emailAddress, supplier);

            var vendorManagerAddress = supplier.VendorManager.Employee.PhoneListEntry.EmailAddress;
            var vendorManagerName = supplier.VendorManager.Employee.FullName;

            var export = this.forecastOrdersReportService.GetMonthlyExport(toSupplier);

            this.emailService.SendEmail(
                test ? ConfigurationManager.Configuration["ORDER_BOOK_TEST_ADDRESS"] : emailAddress,
                supplier.Name,
                null,
                null,
                vendorManagerAddress,
                vendorManagerName,
                $"Monthly Forecast - {timestamp}",
                "Please find Monthly order forecast attached",
                "csv",
                null,
                $"{toSupplier}_monthly_forecast_{timestamp}",
                null,
                export);
        }

        private void CheckEmailDetailsOk(string toAddress, Supplier supplier)
        {
            if (string.IsNullOrEmpty(toAddress))
            {
                throw new SupplierAutoEmailsException($"No recipient address set for: {supplier.Name}");
            }

            var lastTqmsDate = this.mrMaster.GetRecord().RunDate;

            // notify the vendor managers if tqms jobs failed
            if (lastTqmsDate.Date != DateTime.Today.Date)
            {
                var msg = "The Supplier Auto emails could not be sent because the MRP did not run over the weekend.";
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

                throw new SupplierAutoEmailsException(msg);
            }
        }
    }
}
