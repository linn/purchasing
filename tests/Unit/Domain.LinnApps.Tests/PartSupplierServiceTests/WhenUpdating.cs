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

        private readonly Address newAddress = new Address { Id = 1 };

        private readonly OrderMethod newOrderMethod = new OrderMethod { Name = "M1" };

        private readonly Tariff newTariff = new Tariff { Id = 21 };

        private readonly PackagingGroup newPackagingGroup = new PackagingGroup { Id = 77 };

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
                                   DeliveryAddress = new Address(),
                                   Currency = new Currency()
            };
            this.updated = new PartSupplier
                               {
                                   PartNumber = "PART",
                                   SupplierId = 1,
                                   SupplierDesignation = "We updated this to this.",
                                   OrderMethod = this.newOrderMethod,
                                   DeliveryAddress = this.newAddress,
                                   Currency = this.newCurrency,
                                   Tariff = this.newTariff,
                                   PackagingGroup = this.newPackagingGroup
                               };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.CurrencyRepository.FindById(this.newCurrency.Code).Returns(this.newCurrency);
            this.AddressRepository.FindById(this.newAddress.Id).Returns(this.newAddress);
            this.OrderMethodRepository.FindById(this.newOrderMethod.Name).Returns(this.newOrderMethod);
            this.TariffRepository.FindById(this.newTariff.Id).Returns(this.newTariff);
            this.PackagingGroupRepository.FindById(this.newPackagingGroup.Id).Returns(this.newPackagingGroup);
            this.Sut.UpdatePartSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldPerformLookUps()
        {
            this.AddressRepository.Received().FindById(1);
            this.CurrencyRepository.Received().FindById("USD");
            this.OrderMethodRepository.Received().FindById("M1");
        }

        [Test]
        public void ShouldUpdate()
        {
            this.current.SupplierDesignation.Should().Be("We updated this to this.");
            this.current.Currency.Code.Should().Be(this.newCurrency.Code);
            this.current.OrderMethod.Name.Should().Be(this.newOrderMethod.Name);
            this.current.DeliveryAddress.Id.Should().Be(this.newAddress.Id);
            this.current.PackagingGroup.Id.Should().Be(this.newPackagingGroup.Id);
            this.current.Tariff.Id.Should().Be(this.newTariff.Id);
        }
    }
}
