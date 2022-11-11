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

    public class WhenSendingOrderBookEmailAndNoEmailAddressGiven : ContextBase
    {
        private Supplier supplier;

        private SupplierContact contact;

        private string timestamp;

        [SetUp]
        public void SetUp()
        {
            this.contact = new SupplierContact { EmailAddress = "mainordercontact@supplier.com", IsMainOrderContact = "Y" };
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
                },
                SupplierContacts = new List<SupplierContact> { this.contact }
            };

            this.SupplierRepository.FindById(this.supplier.SupplierId).Returns(this.supplier);
            this.MrMaster.GetRecord().Returns(new MrMaster { RunDate = DateTime.Today });
            this.ReportService.GetOrderBookExport(this.supplier.SupplierId).Returns(new ResultsModel());

            this.Sut.SendOrderBookEmail(null, this.supplier.SupplierId, this.timestamp);
        }

        [Test]
        public void ShouldUseMainOrderContactEmail()
        {
            this.EmailService.Received().SendEmail(
                this.contact.EmailAddress,
                this.supplier.Name,
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                Arg.Any<IEnumerable<Dictionary<string, string>>>(),
                this.supplier.VendorManager.Employee.PhoneListEntry.EmailAddress,
                this.supplier.VendorManager.Employee.FullName,
                $"Linn Products Order Book - {this.supplier.Name}",
                "Please find Order Book attached",
                Arg.Is<IEnumerable<Attachment>>(
                    a => a.First().FileName  == $"{this.supplier.SupplierId}_linn_order_book_{this.timestamp}.csv"));
        }
    }
}
