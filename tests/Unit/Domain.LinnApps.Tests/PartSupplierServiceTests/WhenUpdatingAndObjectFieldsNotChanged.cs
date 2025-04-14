using System.Linq.Expressions;
using Linn.Purchasing.Domain.LinnApps.Parts;

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

        private readonly FullAddress newFullAddress = new FullAddress { Id = 1 };

        private readonly OrderMethod newOrderMethod = new OrderMethod { Name = "M1" };

        private PartSupplier current;

        private PartSupplier updated;

        private Part part;

        [SetUp]
        public void SetUp()
        {
            this.current = new PartSupplier
            {
                PartNumber = "PART",
                SupplierId = 1,
                SupplierDesignation = string.Empty,
                OrderMethod = this.newOrderMethod,
                DeliveryFullAddress = this.newFullAddress,
                Currency = this.newCurrency,
                CreatedBy = new Employee { Id = 33087 }
            };
            this.updated = new PartSupplier
            {
                PartNumber = "PART",
                SupplierId = 1,
                SupplierDesignation = "We updated this to this.",
                OrderMethod = this.newOrderMethod,
                DeliveryFullAddress = this.newFullAddress,
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
                DamagesPercent = 19m,
                DeliveryInstructions = "INSTRUCT",
                NotesForBuyer = "NOTES",
                ManufacturerPartNumber = "MPN",
                VendorPartNumber = "VPN",
                CreatedBy = new Employee { Id = 33087 }
            };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(
                    new Part { DecrementRule = "YES", BomType = "B" });


            this.Sut.UpdatePartSupplier(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldNotPerformLookUps()
        {
            this.AddressRepository.DidNotReceive().FindById(Arg.Any<int>());
            this.CurrencyRepository.DidNotReceive().FindById(Arg.Any<string>());
            this.OrderMethodRepository.DidNotReceive().FindById(Arg.Any<string>());
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
            this.current.DamagesPercent.Should().Be(19m);
            this.current.DeliveryInstructions.Should().Be("INSTRUCT");
            this.current.NotesForBuyer.Should().Be("NOTES");
            this.current.ManufacturerPartNumber.Should().Be("MPN");
            this.current.VendorPartNumber.Should().Be("VPN");
            this.current.BaseOurUnitPrice.Should().Be(12m);
            this.current.OurCurrencyPriceToShowOnOrder.Should().Be(11m);
            this.current.CurrencyUnitPrice.Should().Be(10m);
            this.current.SupplierDesignation.Should().Be("We updated this to this.");
            this.current.Currency.Code.Should().Be(this.newCurrency.Code);
            this.current.OrderMethod.Name.Should().Be(this.newOrderMethod.Name);
            this.current.DeliveryFullAddress.Id.Should().Be(this.newFullAddress.Id);
            this.current.DateInvalid.Should().Be(DateTime.UnixEpoch);
        }
    }
}
