namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingPreferredSupplier : ContextBase
    {
        private PreferredSupplierChange candidate;

        private PreferredSupplierChange result;

        private PartSupplier oldPartSupplierRecord;

        private PartSupplier newPartSupplierRecord;

        private Part part;

        [SetUp]
        public void SetUp()
        {
            this.oldPartSupplierRecord = new PartSupplier { PartNumber = "PART", SupplierId = 1 };
            this.newPartSupplierRecord = new PartSupplier { PartNumber = "PART", SupplierId = 2 };

            this.part = new Part
                            {
                                PartNumber = "PART",
                                BomType = "A",
                                PreferredSupplier = new Supplier { SupplierId = 777 },
                                CurrencyUnitPrice = 100m,
                                BaseUnitPrice = 50m,
                                Currency = new Currency { Code = "USD"}
                            };

            this.candidate = new PreferredSupplierChange
                                 {
                                     PartNumber = "PART",
                                     OldSupplier = new Supplier { SupplierId = 1 },
                                     NewSupplier = new Supplier { SupplierId = 2 }
                                 };

            this.MockAuthService.HasPermissionFor(
                    AuthorisedAction.PartSupplierUpdate,
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(this.part);

            this.PartSupplierRepository.FindById(Arg.Any<PartSupplierKey>()).Returns(
                this.oldPartSupplierRecord,
                this.newPartSupplierRecord);

            this.result = this.Sut.CreatePreferredSupplierChange(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.result.PartNumber.Should().Be("PART");
        }

        [Test]
        public void ShouldSetOldSupplierToBeThePartsOldPreferredSupplier()
        {
            this.result.OldSupplier.SupplierId.Should().Be(this.part.PreferredSupplier.SupplierId);
        }

        [Test]
        public void ShouldSetOldPriceToBeThePartsOldCurrencyUnitPrice()
        {
            this.result.OldPrice.Should().Be(this.part.CurrencyUnitPrice);
        }

        [Test]
        public void ShouldSetOldCurrencyToBeThePartsOldCurrencyPrice()
        {
            this.result.OldCurrency.Code.Should().Be(this.part.Currency.Code);
        }

        [Test]
        public void ShouldSetOldBasePriceToBeThePartsOldBaseUnitPrice()
        {
            this.result.BaseOldPrice.Should().Be(this.part.BaseUnitPrice);
        }

        [Test]
        public void ShouldUpdatePartSupplierRecords()
        {
            this.newPartSupplierRecord.SupplierRanking.Should().Be(1);
            this.oldPartSupplierRecord.SupplierRanking.Should().Be(2);
        }
    }
}
