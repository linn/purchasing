namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private readonly Currency newCurrency = new Currency { Code = "USD" };

        private readonly FullAddress newFullAddress = new FullAddress { Id = 1 };

        private readonly OrderMethod newOrderMethod = new OrderMethod { Name = "M1" };

        private readonly PackagingGroup newPackagingGroup = new PackagingGroup { Id = 77 };

        private readonly Employee madeInvalidBy = new Employee { Id = 33087 };

        private readonly Manufacturer manufacturer = new Manufacturer { Code = "MAN" };

        private PartSupplier current;

        private PartSupplier updated;

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
                                   Currency = new Currency(),
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
                                   PackagingGroup = this.newPackagingGroup,
                                   MadeInvalidBy = this.madeInvalidBy,
                                   Manufacturer = this.manufacturer,
                                   CreatedBy = new Employee { Id = 33087 },
                                   MinimumOrderQty = 1m,
                                   LeadTimeWeeks = 1,
                                   OrderIncrement = 1m,
                                   CurrencyUnitPrice = 1m,
                                   DamagesPercent = 0m,
                                   MinimumDeliveryQty = 1m
            };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.CurrencyRepository.FindById(this.newCurrency.Code).Returns(this.newCurrency);
            this.AddressRepository.FindById(this.newFullAddress.Id).Returns(this.newFullAddress);
            this.OrderMethodRepository.FindById(this.newOrderMethod.Name).Returns(this.newOrderMethod);
            this.PackagingGroupRepository.FindById(this.newPackagingGroup.Id).Returns(this.newPackagingGroup);
            this.EmployeeRepository.FindById(this.madeInvalidBy.Id).Returns(this.madeInvalidBy);
            this.ManufacturerRepository.FindById(this.manufacturer.Code).Returns(this.manufacturer);
            this.Sut.UpdatePartSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldPerformLookUps()
        {
            this.AddressRepository.Received().FindById(this.newFullAddress.Id);
            this.CurrencyRepository.Received().FindById(this.newCurrency.Code);
            this.OrderMethodRepository.Received().FindById(this.newOrderMethod.Name);
            this.PackagingGroupRepository.Received().FindById(this.newPackagingGroup.Id);
        }

        [Test]
        public void ShouldUpdate()
        {
            this.current.SupplierDesignation.Should().Be("We updated this to this.");
            this.current.Currency.Code.Should().Be(this.newCurrency.Code);
            this.current.OrderMethod.Name.Should().Be(this.newOrderMethod.Name);
            this.current.DeliveryFullAddress.Id.Should().Be(this.newFullAddress.Id);
            this.current.PackagingGroup.Id.Should().Be(this.newPackagingGroup.Id);
            this.current.MadeInvalidBy.Id.Should().Be(this.madeInvalidBy.Id);
            this.current.Manufacturer.Code.Should().Be(this.manufacturer.Code);
        }
    }
}
