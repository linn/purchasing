namespace Linn.Purchasing.Domain.LinnApps.Tests.PartSupplierServiceTests
{
    using System;
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
                Currency = this.newCurrency,
                DateInvalid = DateTime.UnixEpoch,
                CurrencyUnitPrice = 10m,
                OurCurrencyPriceToShowOnOrder = 11m,
                BaseOurUnitPrice = 12m,
                MinimumDeliveryQty = 13m,
                MinimumOrderQty = 14m,
                OrderIncrement = 15m,
                ReelOrBoxQty = 16m,
                LeadTimeWeeks = 17,
                ContractLeadTimeWeeks = 18,
                OverbookingAllowed = "N",
                DamagesPercent = 19m,
                WebAddress = "/web",
                DeliveryInstructions = "INSTRUCT",
                NotesForBuyer = "NOTES",
                ManufacturerPartNumber = "MPN",
                VendorPartNumber = "VPN",
                RohsCategory = "COMPLIANT",
                DateRohsCompliant = DateTime.UnixEpoch,
                RohsCompliant = "Y",
                RohsComments = "COMMENT"
            };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.Sut.UpdatePartSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldNotPerformLookUps()
        {
            this.AddressRepository.DidNotReceive().FindById(Arg.Any<int>());
            this.CurrencyRepository.DidNotReceive().FindById(Arg.Any<string>());
            this.OrderMethodRepository.DidNotReceive().FindById(Arg.Any<string>());
            this.TariffRepository.DidNotReceive().FindById(Arg.Any<int>());
            this.PackagingGroupRepository.DidNotReceive().FindById(Arg.Any<int>());
            this.EmployeeRepository.DidNotReceive().FindById(Arg.Any<int>());
            this.ManufacturerRepository.DidNotReceive().FindById(Arg.Any<string>());
        }

        [Test]
        public void ShouldUpdateOtherFields()
        {
            this.current.MinimumDeliveryQty.Should().Be(13m);
            this.current.MinimumOrderQty.Should().Be(14m);
            this.current.OrderIncrement.Should().Be(15m);
            this.current.ReelOrBoxQty.Should().Be(16m);
            this.current.LeadTimeWeeks.Should().Be(17);
            this.current.ContractLeadTimeWeeks.Should().Be(18);
            this.current.OverbookingAllowed.Should().Be("N");
            this.current.DamagesPercent.Should().Be(19m);
            this.current.WebAddress.Should().Be("/web");
            this.current.DeliveryInstructions.Should().Be("INSTRUCT");
            this.current.NotesForBuyer.Should().Be("NOTES");
            this.current.ManufacturerPartNumber.Should().Be("MPN");
            this.current.VendorPartNumber.Should().Be("VPN");
            this.current.RohsCategory.Should().Be("COMPLIANT");
            this.current.DateRohsCompliant.Should().Be(DateTime.UnixEpoch);
            this.current.RohsCompliant.Should().Be("Y");
            this.current.RohsComments.Should().Be("COMMENT");
            this.current.BaseOurUnitPrice.Should().Be(12m);
            this.current.OurCurrencyPriceToShowOnOrder.Should().Be(11m);
            this.current.CurrencyUnitPrice.Should().Be(10m);
            this.current.SupplierDesignation.Should().Be("We updated this to this.");
            this.current.Currency.Code.Should().Be(this.newCurrency.Code);
            this.current.OrderMethod.Name.Should().Be(this.newOrderMethod.Name);
            this.current.DeliveryAddress.Id.Should().Be(this.newAddress.Id);
            this.current.DateInvalid.Should().Be(DateTime.UnixEpoch);
        }
    }
}
