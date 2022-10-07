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

    public class WhenCreatingPreferredSupplierChangeForAPartThatAlreadyHasOne : ContextBase
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
                                PreferredSupplier = new Supplier {SupplierId = 1},
                                CurrencyUnitPrice = 100m,
                                BaseUnitPrice = 50m,
                                Currency = new Currency {Code = "USD"},
                                MaterialPrice = 1m,
                                LabourPrice = 2m
                            };

            this.candidate = new PreferredSupplierChange
                                 {
                                     PartNumber = "PART",
                                     OldSupplier = new Supplier {SupplierId = 1},
                                     NewSupplier =
                                         new Supplier
                                             {
                                                 SupplierId = 2,
                                                 VendorManager = new VendorManager {Id = "V"},
                                                 Planner = new Planner {Id = 1}
                                             },
                                     ChangeReason = new PriceChangeReason {ReasonCode = "CHG", Description = "DESC"},
                                     ChangedBy = new Employee {Id = 33087},
                                     Remarks = "REMARKS",
                                     BaseNewPrice = 3m,
                                     NewCurrency = new Currency {Code = "USD"}
                                 };

            this.oldPartSupplierRecord =
                new PartSupplier {PartNumber = "PART", SupplierId = 1, Supplier = this.candidate.OldSupplier};
            this.newPartSupplierRecord =
                new PartSupplier {PartNumber = "PART", SupplierId = 2, Supplier = this.candidate.NewSupplier};

            this.SupplierRepository.FindById(this.candidate.OldSupplier.SupplierId).Returns(this.candidate.OldSupplier);
            this.SupplierRepository.FindById(this.candidate.NewSupplier.SupplierId).Returns(this.candidate.NewSupplier);
            this.EmployeeRepository.FindById(33087).Returns(this.candidate.ChangedBy);
            this.ChangeReasonsRepository.FindById(this.candidate.ChangeReason.ReasonCode)
                .Returns(this.candidate.ChangeReason);

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(this.part);

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
        public void ShouldNotPartPrices()
        {
            this.result.NewCurrency.Code.Should().Be(this.candidate.OldCurrency.Code);
            this.result.BaseNewPrice.Should().Be(this.candidate.BaseOldPrice);
            this.result.NewPrice.Should().Be(this.candidate.OldPrice);
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
        public void ShouldNotUpdatePartPrices()
        {
            this.part.CurrencyUnitPrice.Should().Be(100m);
            this.part.BaseUnitPrice.Should().Be(50m);
            this.part.Currency.Code.Should().Be("USD");
        }

        [Test]
        public void ShouldInsertAPartHistoryRecord()
        {
            this.PartHistory.Received().AddPartHistory(Arg.Any<Part>(), Arg.Any<Part>(), "PREFSUP", Arg.Any<int>(), "REMARKS", "CHG");
        }
    }
}
