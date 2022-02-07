namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : AuthorisedContext
    {
        private Supplier current;

        private Supplier updated;

        private Currency currency;

        private IEnumerable<string> privileges;

        [SetUp]
        public void SetUp()
        {
            this.currency = new Currency { Code = "USD" };
            this.current = new Supplier { SupplierId = 1, Name = "SUPPLIER" };

            this.updated = new Supplier
                               {
                                   Name = "NEW NAME",
                                   SupplierId = 1,
                                   Currency = this.currency,
                                   VendorManager = "V",
                                   WebAddress = "/web",
                                   InvoiceContactMethod = "POST",
                                   LiveOnOracle = "Y",
                                   OrderContactMethod = "EMAIL",
                                   PhoneNumber = "123 456 789",
                                   Planner = 1,
                                   SuppliersReference = "REF",
                                   PaymentDays = 1,
                                   PaymentMethod = "PAYMENT METHOD"
            };
            this.MockCurrencyRepository
                .FindById(this.updated.Currency.Code).Returns(this.currency);
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
            this.current.LiveOnOracle.Should().Be(this.updated.LiveOnOracle);
            this.current.OrderContactMethod.Should().Be(this.updated.OrderContactMethod);
            this.current.InvoiceContactMethod.Should().Be(this.updated.InvoiceContactMethod);
            this.current.PhoneNumber.Should().Be(this.updated.PhoneNumber);
            this.current.Planner.Should().Be(this.updated.Planner);
            this.current.SuppliersReference.Should().Be(this.updated.SuppliersReference);
        }
    }
}
