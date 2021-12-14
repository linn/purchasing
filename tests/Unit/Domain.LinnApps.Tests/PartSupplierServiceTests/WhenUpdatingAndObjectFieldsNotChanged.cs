﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAndObjectFieldsNotChanged : ContextBase
    {
        private readonly Currency newCurrency = new Currency { Code = "USD" };

        private readonly Address newAddress = new Address { Id = 1 };

        private readonly OrderMethod newOrderMethod = new OrderMethod { Name = "M1" };

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
                OrderMethod = this.newOrderMethod,
                DeliveryAddress = this.newAddress,
                Currency = this.newCurrency
            };
            this.updated = new PartSupplier
            {
                PartNumber = "PART",
                SupplierId = 1,
                SupplierDesignation = "We updated this to this.",
                OrderMethod = this.newOrderMethod,
                DeliveryAddress = this.newAddress,
                Currency = this.newCurrency
            };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.CurrencyRepository.FindById("USD").Returns(this.newCurrency);
            this.AddressRepository.FindById(1).Returns(this.newAddress);
            this.OrderMethodRepository.FindById("M1").Returns(this.newOrderMethod);

            this.Sut.UpdatePartSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldNotPerformLookUps()
        {
            this.AddressRepository.DidNotReceive().FindById(1);
            this.CurrencyRepository.DidNotReceive().FindById("USD");
            this.OrderMethodRepository.DidNotReceive().FindById("M1");
        }

        [Test]
        public void ShouldUpdateOtherFields()
        {
            this.current.SupplierDesignation.Should().Be("We updated this to this.");
            this.current.Currency.Code.Should().Be(this.newCurrency.Code);
            this.current.OrderMethod.Name.Should().Be(this.newOrderMethod.Name);
            this.current.DeliveryAddress.Id.Should().Be(this.newAddress.Id);
        }
    }
}
