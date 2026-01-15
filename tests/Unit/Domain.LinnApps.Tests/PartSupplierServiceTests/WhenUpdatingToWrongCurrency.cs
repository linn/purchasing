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

    public class WhenUpdatingToWrongCurrency : ContextBase
    {
        private readonly Currency supplierCurrency = new Currency { Code = "GBP" };
        
        private readonly Currency newCurrency = new Currency { Code = "USD" };

        private readonly FullAddress newFullAddress = new FullAddress { Id = 1 };

        private readonly OrderMethod newOrderMethod = new OrderMethod { Name = "M1" };

        private readonly Employee madeInvalidBy = new Employee { Id = 33087 };

        private readonly Manufacturer manufacturer = new Manufacturer { Code = "MAN" };

        private PartSupplier current;

        private PartSupplier updated;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.current = new PartSupplier
                               {
                                   PartNumber = "PART",
                                   SupplierId = 1,
                                   SupplierDesignation = string.Empty,
                                   OrderMethod = new OrderMethod(),
                                   DeliveryFullAddress = new FullAddress(),
                                   Currency = this.supplierCurrency,
                                   CreatedBy = new Employee { Id = 33087 },
                                   MinimumOrderQty = 1m,
                                   LeadTimeWeeks = 1,
                                   OrderIncrement = 1m,
                                   CurrencyUnitPrice = 1m,
                                   DamagesPercent = 0m,
                                   MinimumDeliveryQty = 1m
            };
            this.updated = new PartSupplier
                               {
                                   PartNumber = "PART",
                                   SupplierId = 1,
                                   SupplierDesignation = "We updated this to this.",
                                   OrderMethod = this.newOrderMethod,
                                   DeliveryFullAddress = this.newFullAddress,
                                   Currency = this.newCurrency,
                                   MadeInvalidBy = this.madeInvalidBy,
                                   Manufacturer = this.manufacturer,
                                   CreatedBy = new Employee { Id = 33087 },
                                   MinimumOrderQty = 1m,
                                   LeadTimeWeeks = 1,
                                   OrderIncrement = 1m,
                                   CurrencyUnitPrice = 1m,
                                   DamagesPercent = 0m,
                                   MinimumDeliveryQty = 1m,
                                   UnitOfMeasure = "NEW"
            };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.CurrencyRepository.FindById(this.newCurrency.Code).Returns(this.newCurrency);
            this.AddressRepository.FindById(this.newFullAddress.Id).Returns(this.newFullAddress);
            this.OrderMethodRepository.FindById(this.newOrderMethod.Name).Returns(this.newOrderMethod);
            this.EmployeeRepository.FindById(this.madeInvalidBy.Id).Returns(this.madeInvalidBy);
            this.ManufacturerRepository.FindById(this.manufacturer.Code).Returns(this.manufacturer);
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(
                    new Part { DecrementRule = "YES", BomType = "B" });
            this.SupplierRepository.FindById(1)
                .Returns(new Supplier { SupplierId = 1, Currency = this.supplierCurrency });

            this.action = () => this.Sut.UpdatePartSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PartSupplierException>()
                .WithMessage("Supplier 1 has currency GBP. Cannot update part supplier to currency USD.");
        }
    }
}
