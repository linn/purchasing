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

    public class WhenCreatingPreferredSupplierChangeForPartThatWasZeroPricedButHasStock : ContextBase
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
                LinnProduced = "N",
                CurrencyUnitPrice = 0,
                BaseUnitPrice = 0,
                Currency = new Currency { Code = "USD" },
                MaterialPrice = 0,
                LabourPrice = 0
            };

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
                NewCurrency = new Currency { Code = "USD" }
            };

            this.oldPartSupplierRecord =
                new PartSupplier { PartNumber = "PART", SupplierId = 1, Supplier = this.candidate.OldSupplier };
            this.newPartSupplierRecord =
                new PartSupplier { PartNumber = "PART", SupplierId = 2, Supplier = this.candidate.NewSupplier };

            this.SupplierRepository.FindById(this.candidate.OldSupplier.SupplierId).Returns(this.candidate.OldSupplier);
            this.SupplierRepository.FindById(this.candidate.NewSupplier.SupplierId).Returns(this.candidate.NewSupplier);
            this.EmployeeRepository.FindById(33087).Returns(this.candidate.ChangedBy);
            this.ChangeReasonsRepository.FindById(this.candidate.ChangeReason.ReasonCode)
                .Returns(this.candidate.ChangeReason);

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(this.part);

            var stock = new List<StockLocator>() { new StockLocator { Id = 1, Qty = 1 } };
            this.StockLocatorRepository.FilterBy(Arg.Any<Expression<Func<StockLocator, bool>>>()).Returns(stock.AsQueryable());

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
        public void ShouldNotUpdatePartPrices()
        {
            this.part.CurrencyUnitPrice.Should().Be(0);
            this.part.BaseUnitPrice.Should().Be(0);
        }
    }
}
