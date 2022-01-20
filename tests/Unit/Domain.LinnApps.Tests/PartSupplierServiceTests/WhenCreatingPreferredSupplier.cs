namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;
    using NSubstitute.ReceivedExtensions;

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
          
            this.part = new Part
                            {
                                PartNumber = "PART",
                                BomType = "A",
                                PreferredSupplier = new Supplier { SupplierId = 1 },
                                CurrencyUnitPrice = 100m,
                                BaseUnitPrice = 50m,
                                Currency = new Currency { Code = "USD" },
                                MaterialPrice = 1m,
                                LabourPrice = 2m
                            };

            this.candidate = new PreferredSupplierChange
                                 {
                                     PartNumber = "PART",
                                     OldSupplier = new Supplier { SupplierId = 1 },
                                     NewSupplier = new Supplier { SupplierId = 2 },
                                     ChangeReason = new PriceChangeReason { ReasonCode = "CHG", Description = "DESC" },
                                     ChangedBy = new Employee { Id = 33087 },
                                     Remarks = "REMARKS",
                                     BaseNewPrice = 3m,
                                     NewCurrency = new Currency { Code = "USD" }
                                 };

            this.oldPartSupplierRecord = new PartSupplier { PartNumber = "PART", SupplierId = 1, Supplier = this.candidate.OldSupplier };
            this.newPartSupplierRecord = new PartSupplier { PartNumber = "PART", SupplierId = 2, Supplier = this.candidate.NewSupplier };

            this.PartHistory.FilterBy(Arg.Any<Expression<Func<PartHistoryEntry, bool>>>())
                .Returns(new List<PartHistoryEntry>().AsQueryable());

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
            this.result.OldSupplier.SupplierId.Should().Be(this.oldPartSupplierRecord.SupplierId);
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

        [Test]
        public void ShouldInsertAPartHistoryRecord()
        {
            this.PartHistory.Received().Add(Arg.Is<PartHistoryEntry>(x => 
                x.Seq  == 1
                && x.PartNumber == "PART"
                && x.OldMaterialPrice == 1m
                && x.OldLabourPrice == 2m
                && x.NewMaterialPrice == 1m
                && x.NewLabourPrice == 0m
                && x.OldPreferredSupplierId == 1
                && x.NewPreferredSupplierId == 2
                && x.OldBomType == "A"
                && x.NewBomType == "A"
                && x.ChangedBy == 33087
                && x.ChangeType == "PREFSUP"
                && x.Remarks == "REMARKS"
                && x.PriceChangeReason == "CHG"
                && x.OldCurrency == "USD"
                && x.NewCurrency == "USD"
                && x.OldCurrencyUnitPrice == 100m
                && x.NewCurrencyUnitPrice == 100m
                && x.OldBaseUnitPrice == 50m
                && x.NewBaseUnitPrice == 50m));
        }
    }
}
