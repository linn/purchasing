namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAndInvalidFields : AuthorisedContext
    {
        private Supplier current;

        private Supplier updated;

        private Currency currency;

        private Supplier otherSupplier;

        private PartCategory partCategory;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.currency = new Currency { Code = "USD" };
            this.current = new Supplier { SupplierId = 1, Name = "SUPPLIER" };

            this.otherSupplier = new Supplier { SupplierId = 2, Name = "SUPPLIER 2" };

            this.partCategory = new PartCategory { Category = "CAT" };


            this.updated = new Supplier
            {
                Currency = this.currency,
                VendorManager = new VendorManager { VmId = "V"},
                WebAddress = "/web",
                LiveOnOracle = "Y",
                OrderContactMethod = "EMAIL",
                PhoneNumber = "123 456 789",
                Planner = new Planner { Id = 1 },
                SuppliersReference = "REF",
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

            this.action = () => this.Sut.UpdateSupplier(
                this.current, 
                this.updated, 
                new List<string>());
        }

        [Test]
        public void ShouldNotUpdate()
        {
            this.current.Name.Should().Be("SUPPLIER");
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<SupplierException>()
                .WithMessage(
                    "The inputs for the following fields are empty/invalid: Supplier Id, "
                    + "Supplier Name, Invoice Contact Method, Payment Days, Payment Method, ");
        }
    }
}
