namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenUpdatingAndNoBaseUnitPriceSupplied : ContextBase
    {
        private Action action;

        private PartSupplier current;

        private PartSupplier updated;

        [SetUp]
        public void SetUp()
        {
            var currency = new Currency() { Code = "GBP", Name = "Great British Sterling" };
            this.CurrencyRepository.FindById(currency.Code).Returns(currency);

            this.current = new PartSupplier
            {
                PartNumber = "PART",
                SupplierId = 1,
                SupplierDesignation = "A PART",
                OrderMethod = new OrderMethod(),
                DeliveryFullAddress = new FullAddress(),
                Currency = currency,
                CreatedBy = new Employee { Id = 33087 },
                MinimumOrderQty = 1m,
                LeadTimeWeeks = 1,
                OrderIncrement = 1m,
                CurrencyUnitPrice = 1m,
                BaseOurUnitPrice = 1m,
                DamagesPercent = 0m,
                MinimumDeliveryQty = 1m,
                UnitOfMeasure = "NEW"
            };
            this.updated = new PartSupplier
            {
                PartNumber = "PART",
                SupplierId = 1,
                SupplierDesignation = "A PART",
                OrderMethod = new OrderMethod(),
                DeliveryFullAddress = new FullAddress(),
                Currency = currency,
                CreatedBy = new Employee { Id = 33087 },
                MinimumOrderQty = 1m,
                LeadTimeWeeks = 1,
                OrderIncrement = 1m,
                CurrencyUnitPrice = 2m,
                DamagesPercent = 0m,
                MinimumDeliveryQty = 1m,
                UnitOfMeasure = "NEW"
            };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(
                    new Part
                    {
                        PreferredSupplier = new Supplier
                        {
                            SupplierId = 1
                        },
                        DecrementRule = "YES", 
                        BomType = "C"
                    });

            this.action = () => this.Sut.UpdatePartSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldNotUpdate()
        {
            this.current.CurrencyUnitPrice.Should().Be(1m);
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PartSupplierException>();
        }

        [Test]
        public void ShouldListMissingFieldsInMessage()
        {
            this.action.Should().Throw<PartSupplierException>()
                .WithMessage(
                    "No Base Our Unit Price Supplied - please tell IT Support");
        }
    }
}
