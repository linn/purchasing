namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : AuthorisedContext
    {
        private Supplier current;

        private Supplier updated;

        private Currency currency;

        private Supplier otherSupplier;

        private PartCategory partCategory;

        private IEnumerable<string> privileges;

        [SetUp]
        public void SetUp()
        {
            this.currency = new Currency { Code = "USD" };
            this.current = new Supplier { SupplierId = 1, Name = "SUPPLIER" };

            this.otherSupplier = new Supplier { SupplierId = 2, Name = "SUPPLIER 2" };

            this.partCategory = new PartCategory { Category = "CAT" };

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
                                   PaymentMethod = "PAYMENT METHOD",
                                   InvoiceGoesTo = this.otherSupplier,
                                   ExpenseAccount = "Y",
                                   PaysInFc = "Y",
                                   ApprovedCarrier = "Y",
                                   AccountingCompany = "LINN",
                                   VatNumber = "012345",
                                   PartCategory = this.partCategory,
                                   OrderHold = "Y",
                                   NotesForBuyer = "NOTES",
                                   DeliveryDay = "FRIDAY",
                                   RefersToFc = this.otherSupplier,
                                   PmDeliveryDaysGrace = 1
                               };
            this.MockCurrencyRepository
                .FindById(this.updated.Currency.Code).Returns(this.currency);
            this.MockSupplierRepository.FindById(2).Returns(this.otherSupplier);
            this.MockPartCategoryRepository.FindById("CAT").Returns(this.partCategory);
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
            this.current.PaymentDays.Should().Be(this.updated.PaymentDays);
            this.current.InvoiceGoesTo.SupplierId.Should().Be(this.otherSupplier.SupplierId);
            this.current.PaymentMethod.Should().Be(this.updated.PaymentMethod);
            this.current.ExpenseAccount.Should().Be(this.updated.ExpenseAccount);
            this.current.PaysInFc.Should().Be(this.updated.PaysInFc);
            this.current.ApprovedCarrier.Should().Be(this.updated.ApprovedCarrier);
            this.current.AccountingCompany.Should().Be(this.updated.AccountingCompany);
            this.current.VatNumber.Should().Be(this.updated.VatNumber);
            this.current.PartCategory.Category.Should().Be(this.partCategory.Category);
            this.current.OrderHold.Should().Be(this.updated.OrderHold);
            this.current.NotesForBuyer.Should().Be(this.updated.NotesForBuyer);
            this.current.DeliveryDay.Should().Be(this.updated.DeliveryDay);
            this.current.RefersToFc.SupplierId.Should().Be(this.otherSupplier.SupplierId);
            this.current.PmDeliveryDaysGrace.Should().Be(this.updated.PmDeliveryDaysGrace);
        }
    }
}
