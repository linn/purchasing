namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests.SwitchQtyPriceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NUnit.Framework;

    public class WhenSwitchingQtyAndPrice : SwitchQtyPriceContextBase
    {
        private PurchaseOrder result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.SwitchOurQtyAndPrice(this.OrderNumber, 1, this.EmployeeId, new List<string>());
        }

        [Test]
        public void ShouldReturnOrder()
        {
            this.result.OrderNumber.Should().Be(this.OrderNumber);
        }

        [Test]
        public void ShouldUpdateOrderDetails()
        {
            var updatedLine = this.PurchaseOrder.Details.First();
            updatedLine.OurQty.Should().Be(3800m);
            updatedLine.OurUnitPriceCurrency.Should().Be(1m);
            updatedLine.BaseOurUnitPrice.Should().Be(1m);
            updatedLine.OrderConversionFactor.Should().Be(3800m);

            updatedLine.OrderPosting.Qty.Should().Be(3800m);
         
            var delivery = updatedLine.PurchaseDeliveries.First();
            delivery.OurDeliveryQty.Should().Be(3800m);
            delivery.OurUnitPriceCurrency.Should().Be(1m);
            delivery.BaseOurUnitPrice.Should().Be(1m);
        }

        [Test]
        public void ShouldNotUpdateOrderQuantitiesOrPrices()
        {
            var updatedLine = this.PurchaseOrder.Details.First();
            updatedLine.OrderQty.Should().Be(1m);
            updatedLine.OrderUnitPriceCurrency.Should().Be(3800m);
            updatedLine.BaseOrderUnitPrice.Should().Be(3800m);

            var delivery = updatedLine.PurchaseDeliveries.First();
            delivery.OrderDeliveryQty.Should().Be(1m);
            delivery.OrderUnitPriceCurrency.Should().Be(3800m);
            delivery.BaseOrderUnitPrice.Should().Be(3800m);
        }
    }
}
