namespace Linn.Purchasing.Domain.LinnApps.Tests.MrOrderBookMailer
{
    using System;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSendingEmailAndNoEmailAddressSuppliedAndSupplierHasNoMainOrderContact 
        : ContextBase
    {
        private Supplier supplier;

        private Action action;

        private string timestamp;

        [SetUp]
        public void SetUp()
        {
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

            this.action  = () =>
                this.Sut.SendOrderBookEmail(null, this.supplier.SupplierId, this.timestamp);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<MrOrderBookEmailException>()
                .WithMessage($"No recipient address set for: {this.supplier.Name}");
        }
    }
}
