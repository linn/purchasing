﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
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

    public class WhenCreatingAPreferredSupplierChangeForAPartForTheFirstTime : ContextBase
    {
        private PreferredSupplierChange candidate;

        private PreferredSupplierChange result;

        private PartSupplier newPartSupplierRecord;

        private Part part;

        [SetUp]
        public void SetUp()
        {
            this.part = new Part { PartNumber = "PART", BomType = "A", PreferredSupplier = null };

            this.candidate = new PreferredSupplierChange
                                 {
                                     PartNumber = "PART",
                                     OldSupplier = new Supplier { SupplierId = 1 },
                                     NewSupplier =
                                         new Supplier
                                             {
                                                 SupplierId = 2,
                                                 VendorManager = new VendorManager { Id = "V" },
                                                 Planner = new Planner { Id = 1 }
                                             },
                                     ChangeReason = new PriceChangeReason { ReasonCode = "CHG", Description = "DESC" },
                                     ChangedBy = new Employee { Id = 33087 },
                                     Remarks = "REMARKS",
                                     BaseNewPrice = 3m,
                                     NewPrice = 100m,
                                     NewCurrency = new Currency { Code = "USD" }
                                 };

            this.newPartSupplierRecord = new PartSupplier
                                             {
                                                 PartNumber = "PART",
                                                 SupplierId = 2,
                                                 Supplier = this.candidate.NewSupplier
                                             };

            this.SupplierRepository.FindById(this.candidate.OldSupplier.SupplierId).Returns(this.candidate.OldSupplier);
            this.SupplierRepository.FindById(this.candidate.NewSupplier.SupplierId).Returns(this.candidate.NewSupplier);
            this.EmployeeRepository.FindById(33087).Returns(this.candidate.ChangedBy);
            this.ChangeReasonsRepository.FindById(this.candidate.ChangeReason.ReasonCode)
                .Returns(this.candidate.ChangeReason);
            this.CurrencyRepository.FindById("USD").Returns(new Currency { Code = "USD" });

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(this.part);

            this.PartSupplierRepository.FindById(Arg.Any<PartSupplierKey>()).Returns(null, this.newPartSupplierRecord);

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
        }

        [Test]
        public void ShouldInsertAPartHistoryRecord()
        {
            this.PartHistory.Received().AddPartHistory(
                Arg.Any<Part>(),
                Arg.Any<Part>(),
                "PREFSUP",
                Arg.Any<int>(),
                "REMARKS",
                "CHG");
        }
    }
}
