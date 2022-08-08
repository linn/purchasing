namespace Linn.Purchasing.Domain.LinnApps.Tests.MrOrderBookMailer
{
    using System;
    using System.IO;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingEmail : ContextBase
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
            this.TqmsMaster.GetRecord().Returns(new TqmsMaster { DateLastDoTqmsSums = DateTime.Today });
            this.ReportService.GetOrderBookExport(this.supplier.SupplierId).Returns(new ResultsModel());

            this.Sut.SendOrderBookEmail(this.email, this.supplier.SupplierId, this.timestamp);
        }

        [Test]
        public void ShouldSendEmail()
        {
            this.EmailService.Received().SendEmail(
                this.email,
                this.supplier.Name,
                null,
                null,
                this.supplier.VendorManager.Employee.PhoneListEntry.EmailAddress,
                this.supplier.VendorManager.Employee.FullName,
                $"MR Order Book - {timestamp}",
                "Please find Order Book attached",
                "csv",
                Arg.Any<Stream>(),
                $"{this.supplier.SupplierId}_linn_order_book_{this.timestamp}");
        }
    }
}
