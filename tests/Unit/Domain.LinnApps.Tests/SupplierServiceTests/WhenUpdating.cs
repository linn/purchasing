namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NUnit.Framework;

    public class WhenUpdating : AuthorisedContext
    {
        private Supplier current;

        private Supplier updated;

        private IEnumerable<string> privileges;

        [SetUp]
        public void SetUp()
        {
            this.current = new Supplier { SupplierId = 1, Name = "SUPPLIER" };

            this.updated = new Supplier
                               {
                                   Name = "NEW NAME",
                                   Currency = new Currency { Code = "USD" },
                                   VendorManager = "V",
                                   WebAddress = "/web",
                                   InvoiceContactMethod = "POST",
                                   LedgerStream = 1,
                                   LiveOnOracle = "Y",
                                   OrderContactMethod = "EMAIL",
                                   PhoneNumber = "123 456 789",
                                   Planner = 1,
                                   SuppliersReference = "REF"
                               };

            this.privileges = new List<string> { "priv" };
            this.Sut.UpdateSupplier(this.current, this.updated, this.privileges);
        }

        [Test]
        public void ShouldNotUpdateId()
        {
            this.current.SupplierId.Should().Be(1);
        }

        [Test]
        public void ShouldUpdateOtherFields()
        {
            this.current.Name.Should().Be(this.updated.Name);
            this.current.Currency.Should().Be(this.updated.Currency);
            this.current.VendorManager.Should().Be(this.updated.VendorManager);
            this.current.InvoiceContactMethod.Should().Be(this.updated.InvoiceContactMethod);
            this.current.LedgerStream.Should().Be(this.updated.LedgerStream);
            this.current.LiveOnOracle.Should().Be(this.updated.LiveOnOracle);
            this.current.OrderContactMethod.Should().Be(this.updated.OrderContactMethod);
            this.current.InvoiceContactMethod.Should().Be(this.updated.InvoiceContactMethod);
            this.current.PhoneNumber.Should().Be(this.updated.PhoneNumber);
            this.current.Planner.Should().Be(this.updated.Planner);
            this.current.SuppliersReference.Should().Be(this.updated.SuppliersReference);
        }
    }
}
