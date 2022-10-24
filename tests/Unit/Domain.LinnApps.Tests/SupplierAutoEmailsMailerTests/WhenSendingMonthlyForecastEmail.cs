namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierAutoEmailsMailerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Email;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingMonthlyForecastEmail : ContextBase
    {
        private Supplier supplier;

        private string email;

        private string timestamp;

        [SetUp]
        public void SetUp()
        {
            this.email = "supplier@email.com";
            this.timestamp = DateTime.Today.ToShortTimeString();
            this.supplier = new Supplier
            {
                SupplierId = 1,
                Name = "BIG SUPPLIER",
                VendorManager = new VendorManager
                {
                    Employee = new Employee
                    {
                        FullName = "Test McPerson",
                        PhoneListEntry = new PhoneListEntry
                        {
                            EmailAddress = "test@mcperson.com"
                        }
                    }
                }
            };

            this.SupplierRepository.FindById(this.supplier.SupplierId).Returns(this.supplier);
            this.MrMaster.GetRecord().Returns(new MrMaster { RunDate = DateTime.Today });
            this.ReportService.GetOrderBookExport(this.supplier.SupplierId).Returns(new ResultsModel());

            this.Sut.SendMonthlyForecastEmail(this.email, this.supplier.SupplierId, this.timestamp);
        }

        [Test]
        public void ShouldSendEmailToSupplier()
        {
            this.EmailService.Received().SendEmail(
                this.email,
                this.supplier.Name,
                null,
                null,
                this.supplier.VendorManager.Employee.PhoneListEntry.EmailAddress,
                this.supplier.VendorManager.Employee.FullName,
                $"Monthly Forecast - {timestamp}",
                "Please find Monthly order forecast attached",
                Arg.Is<IEnumerable<Attachment>>(a => a.First().FileName 
                                                     == $"{this.supplier.SupplierId}_monthly_forecast_{this.timestamp}.csv"));
        }
    }
}
