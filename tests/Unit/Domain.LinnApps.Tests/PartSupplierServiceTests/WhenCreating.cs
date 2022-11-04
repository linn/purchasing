namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private PartSupplier candidate;

        private PartSupplier result;

        [SetUp]
        public void SetUp()
        {
            this.candidate = new PartSupplier
                                 {
                                     PartNumber = "PART", 
                                     SupplierId = 1, 
                                     MinimumOrderQty = 10,
                                     CreatedBy = new Employee { Id = 33087 },
                                     OrderIncrement = 1m,
                                     LeadTimeWeeks = 1,
                                     DateCreated = DateTime.UnixEpoch,
                                     SupplierDesignation = "1234567",
                                     CurrencyUnitPrice = 1m,
                                     DamagesPercent = 0m,
                                     MinimumDeliveryQty = 1m,
                                     OrderMethod = new OrderMethod { Name = "METHOD" },
                                     Part = new Part { PartNumber = "PART" },
                                     Currency = new Currency { Code = "GBP" }
            };

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "PART", Description = "DESC" });
            this.SupplierRepository.FindById(1)
                .Returns(new Supplier { SupplierId = 1, Name = "A SUPPLIER" });
            this.MockAuthService.HasPermissionFor(
                    AuthorisedAction.PartSupplierCreate, 
                    Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.result = this.Sut.CreatePartSupplier(this.candidate, new List<string>());
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.result.SupplierId.Should().Be(1);
            this.result.PartNumber.Should().Be("PART");
            this.result.SupplierDesignation.Should().Be("1234567");
            this.result.OverbookingAllowed.Should().Be("Y");
            this.result.Supplier.Name.Should().Be("A SUPPLIER");
        }
    }
}
