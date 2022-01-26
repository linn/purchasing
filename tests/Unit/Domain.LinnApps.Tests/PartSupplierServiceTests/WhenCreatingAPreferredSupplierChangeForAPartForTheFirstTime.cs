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

    using NUnit.Framework;

    public class WhenCreatingAPreferredSupplierChangeForAPartForTheFirstTime : ContextBase
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
                PreferredSupplier = null
            };

            this.candidate = new PreferredSupplierChange
            {
                PartNumber = "PART",
                OldSupplier = new Supplier { SupplierId = 1 },
                NewSupplier = new Supplier { SupplierId = 2, VendorManager = "A", Planner = 1 },
                ChangeReason = new PriceChangeReason { ReasonCode = "CHG", Description = "DESC" },
                ChangedBy = new Employee { Id = 33087 },
                Remarks = "REMARKS",
                BaseNewPrice = 3m,
                NewPrice = 100m,
                NewCurrency = new Currency { Code = "USD" }
            };

            this.oldPartSupplierRecord = new PartSupplier { PartNumber = "PART", SupplierId = 1, Supplier = this.candidate.OldSupplier };
            this.newPartSupplierRecord = new PartSupplier { PartNumber = "PART", SupplierId = 2, Supplier = this.candidate.NewSupplier };

            this.SupplierRepository.FindById(this.candidate.OldSupplier.SupplierId).Returns(this.candidate.OldSupplier);
            this.SupplierRepository.FindById(this.candidate.NewSupplier.SupplierId).Returns(this.candidate.NewSupplier);
            this.EmployeeRepository.FindById(33087).Returns(this.candidate.ChangedBy);
            this.ChangeReasonsRepository.FindById(this.candidate.ChangeReason.ReasonCode)
                .Returns(this.candidate.ChangeReason);
            this.PartHistory.FilterBy(Arg.Any<Expression<Func<PartHistoryEntry, bool>>>())
                .Returns(new List<PartHistoryEntry>().AsQueryable());
            this.CurrencyRepository.FindById("USD").Returns(new Currency { Code = "USD" });

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
        public void ShouldSetOldSupplierToBeNull()
        {
            this.result.OldSupplier.Should().Be(null);
        }

        [Test]
        public void ShouldSetOldPriceToBeNull()
        {
            this.result.OldPrice.Should().Be(null);
        }

        [Test]
        public void ShouldSetOldCurrencyToBeThePartsOldCurrencyPrice()
        {
            this.result.OldCurrency.Should().Be(null);
        }

        [Test]
        public void ShouldSetPartPrices()
        {
            this.result.NewCurrency.Code.Should().Be(this.candidate.NewCurrency.Code);
            this.result.BaseNewPrice.Should().Be(this.candidate.BaseNewPrice);
            this.result.NewPrice.Should().Be(this.candidate.NewPrice);
        }

        [Test]
        public void ShouldSetOldBasePriceToBeNull()
        {
            this.result.BaseOldPrice.Should().Be(null);
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
                x.Seq == 1
                && x.PartNumber == "PART"
                && x.OldMaterialPrice == null
                && x.OldLabourPrice == null
                && x.NewMaterialPrice == 3m
                && x.NewLabourPrice == 0m
                && x.OldPreferredSupplierId == null
                && x.NewPreferredSupplierId == 2
                && x.OldBomType == "A"
                && x.NewBomType == "A"
                && x.ChangedBy == 33087
                && x.ChangeType == "PREFSUP"
                && x.Remarks == "REMARKS"
                && x.PriceChangeReason == "CHG"
                && x.OldCurrency == null
                && x.NewCurrency == "USD"
                && x.OldCurrencyUnitPrice == null
                && x.NewCurrencyUnitPrice == 100m
                && x.OldBaseUnitPrice == null
                && x.NewBaseUnitPrice == 3m));
        }
    }
}
