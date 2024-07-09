namespace Linn.Purchasing.Domain.LinnApps.Mailers
{
    using System;
    using System.Collections.Generic;
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

        public void SendOrderBookEmail(
            string toAddresses, int toSupplier, string timestamp, bool test = false, bool? bypassMrpCheck = false)
        {
            var supplier = this.supplierRepository.FindById(toSupplier);

            var emailAddresses = string.IsNullOrEmpty(toAddresses) ? supplier.SupplierContacts
                ?.First(c => c.IsMainOrderContact.Equals("Y"))?.EmailAddress : toAddresses;

            this.CheckEmailDetailsOk(emailAddresses, supplier, bypassMrpCheck.GetValueOrDefault());
            
            var vendorManagerAddress = supplier.VendorManager.Employee.PhoneListEntry?.EmailAddress;
            var vendorManagerName = supplier.VendorManager.Employee.FullName;

            var export = this.orderBookReportService.GetOrderBookExport(toSupplier);

            foreach (var address in emailAddresses.Split(","))
            {
                if (!string.IsNullOrWhiteSpace(address))
                {
                    var bcc = new List<Dictionary<string, string>>
                                  {
                                      new Dictionary<string, string>
                                          {
                                              { "name", supplier.Name },
                                              { "address", ConfigurationManager.Configuration["ACKNOWLEDGEMENTS_BCC"] }
                                          }
                                  };
                    this.emailService.SendEmail(
                        test ? ConfigurationManager.Configuration["ORDER_BOOK_TEST_ADDRESS"] : address.Trim(),
                        supplier.Name,
                        null,
                        bcc,
                        vendorManagerAddress ?? ConfigurationManager.Configuration["PURCHASING_FROM_ADDRESS"],
                        vendorManagerName != "No person assigned" ? vendorManagerName : "Linn",
                        $"Linn Products Order Book - {supplier.Name}",
                        "Please find Order Book attached",
                        new List<Attachment>
                            {
                                new CsvAttachment(export, null, $"{toSupplier}_linn_order_book_{timestamp}")
                            });
                }
            }
        }

        public void SendMonthlyForecastEmail(
            string toAddresses, int toSupplier, string timestamp, bool test = false, bool? bypassMrpCheck = false)
        {
            var supplier = this.supplierRepository.FindById(toSupplier);

            var emailAddresses = string.IsNullOrEmpty(toAddresses) ? supplier.SupplierContacts
                                   ?.First(c => c.IsMainOrderContact.Equals("Y"))?.EmailAddress : toAddresses;

            this.CheckEmailDetailsOk(emailAddresses, supplier, bypassMrpCheck.GetValueOrDefault());

            var vendorManagerAddress = supplier.VendorManager.Employee.PhoneListEntry.EmailAddress;
            var vendorManagerName = supplier.VendorManager.Employee.FullName;

            var export = this.forecastOrdersReportService.GetMonthlyExport(toSupplier);

            foreach (var address in emailAddresses.Split(","))
            {
                if (!string.IsNullOrWhiteSpace(address))
                {
                    // this.emailService.SendEmail(
                    //     test ? ConfigurationManager.Configuration["ORDER_BOOK_TEST_ADDRESS"] : address.Trim(),
                    //     supplier.Name,
                    //     null,
                    //     null,
                    //     vendorManagerAddress,
                    //     vendorManagerName,
                    //     $"Monthly Forecast - {timestamp}",
                    //     "Please find Monthly order forecast attached",
                    //     new List<Attachment>
                    //         {
                    //             new CsvAttachment(null, export, $"{toSupplier}_monthly_forecast_{timestamp}")
                    //         });
                }
            }
        }

        private void CheckEmailDetailsOk(string toAddresses, Supplier supplier, bool bypassMrpCheck)
        {
            if (string.IsNullOrEmpty(toAddresses))
            {
                throw new SupplierAutoEmailsException($"No recipient address set for: {supplier.Name}");
            }

            // notify the vendor managers if mrp jobs failed
            if (!bypassMrpCheck && this.mrMaster.GetRecord().RunDate.Date != DateTime.Today.Date)
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
                    msg);

                throw new SupplierAutoEmailsException(msg);
            }
        }
    }
}
