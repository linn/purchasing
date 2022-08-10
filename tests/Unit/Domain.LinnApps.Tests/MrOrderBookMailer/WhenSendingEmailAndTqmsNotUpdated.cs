namespace Linn.Purchasing.Domain.LinnApps.Tests.MrOrderBookMailer
{
    using System;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;
    using NSubstitute.ExceptionExtensions;

    using NUnit.Framework;

    public class WhenSendingEmailAndTqmsNotUpdated : ContextBase
    {
        private Supplier supplier;

        private string email;

        private string timestamp;

        private Action action;

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
            this.TqmsMaster.GetRecord().Returns(new TqmsMaster { DateLastDoTqmsSums = DateTime.UnixEpoch });
            this.ReportService.GetOrderBookExport(this.supplier.SupplierId).Returns(new ResultsModel());

            this.action = () => this.Sut.SendOrderBookEmail(this.email, this.supplier.SupplierId, this.timestamp);
        }

        [Test]
        public void ShouldSendAlertToVendorManagerAndThrow()
        {
            this.action.Should().Throw<MrOrderBookEmailException>()
                .WithMessage("The MR Order book emails could not be sent because the TQMS jobs did not run over the weekend.");
            this.EmailService.Received().SendEmail(
                this.supplier.VendorManager.Employee.PhoneListEntry.EmailAddress,
                this.supplier.VendorManager.Employee.FullName,
                null,
                null,
                Arg.Any<string>(),
                "Purchasing Outgoing",
                "MR ORDER BOOK EMAIL ERROR",
                "The MR Order book emails could not be sent because the TQMS jobs did not run over the weekend.",
                null,
                null,
                null);
        }
    }
}
