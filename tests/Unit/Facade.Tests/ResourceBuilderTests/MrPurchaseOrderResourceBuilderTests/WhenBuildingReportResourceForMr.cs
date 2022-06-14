namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.MrPurchaseOrderResourceBuilderTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    using NUnit.Framework;

    public class WhenBuildingResource : ContextBase
    {
        private MrPurchaseOrderResource result;

        private MrPurchaseOrderDetail order;

        private MrPurchaseOrderDelivery delivery;

        [SetUp]
        public void SetUp()
        {
            this.delivery = new MrPurchaseOrderDelivery
                                {
                                    JobRef = this.JobRef,
                                    OrderNumber = 123,
                                    OrderLine = 1,
                                    DeliverySequence = 1,
                                    Quantity = 12,
                                    QuantityReceived = 6,
                                    RequestedDeliveryDate = 1.July(2028),
                                    AdvisedDeliveryDate = 11.July(2028),
                                    Reference = "r1"
                                };
            this.order = new MrPurchaseOrderDetail
                             {
                                 JobRef = this.JobRef,
                                 OrderNumber = 123,
                                 OrderLine = 1,
                                 DateOfOrder = 1.June(2028),
                                 PartNumber = "P1",
                                 OurQuantity = 12,
                                 QuantityReceived = 6,
                                 QuantityInvoiced = 2,
                                 SupplierId = 789,
                                 SupplierName = "S1",
                                 SupplierContact = "Jo",
                                 Remarks = null,
                                 AuthorisedBy = "JK",
                                 Deliveries = new List<MrPurchaseOrderDelivery>
                                                  {
                                                      this.delivery
                                                  }
                             };
            this.result = (MrPurchaseOrderResource)this.Sut.Build(this.order, new List<string>());
        }

        [Test]
        public void ShouldCreateResource()
        {
            this.result.OrderNumber.Should().Be(this.order.OrderNumber);
            this.result.OrderLine.Should().Be(this.order.OrderLine);
            this.result.DateOfOrder.Should().Be(this.order.DateOfOrder.ToString("o"));
            this.result.JobRef.Should().Be(this.order.JobRef);
            this.result.Quantity.Should().Be(this.order.OurQuantity);
            this.result.QuantityInvoiced.Should().Be(this.order.QuantityInvoiced);
            this.result.QuantityReceived.Should().Be(this.order.QuantityReceived);
            this.result.Remarks.Should().Be(this.order.Remarks);
            this.result.SupplierId.Should().Be(this.order.SupplierId);
            this.result.SupplierName.Should().Be(this.order.SupplierName);
            this.result.SupplierContact.Should().Be(this.order.SupplierContact);
            this.result.UnauthorisedWarning.Should().BeNull();
        }

        [Test]
        public void ShouldIncludeDelivery()
        {
            var deliveryResource = this.result.Deliveries.First();
            deliveryResource.OrderNumber.Should().Be(this.delivery.OrderNumber);
            deliveryResource.OrderLine.Should().Be(this.delivery.OrderLine);
            deliveryResource.DeliverySequence.Should().Be(this.delivery.DeliverySequence);
            deliveryResource.JobRef.Should().Be(this.delivery.JobRef);
            deliveryResource.DeliveryQuantity.Should().Be(this.delivery.Quantity);
            deliveryResource.QuantityReceived.Should().Be(this.delivery.QuantityReceived);
            deliveryResource.Reference.Should().Be(this.delivery.Reference);
            deliveryResource.RequestedDeliveryDate.Should().Be(this.delivery.RequestedDeliveryDate?.ToString("o"));
            deliveryResource.AdvisedDeliveryDate.Should().Be(this.delivery.AdvisedDeliveryDate?.ToString("o"));
        }

        [Test]
        public void ShouldReturnCorrectHeaderLinks()
        {
            this.result.Links.Should().Contain(
                a => a.Rel == "view-order" && a.Href == $"/purchasing/purchase-orders/{this.order.OrderNumber}");
            this.result.Links.Should().Contain(
                a => a.Rel == "acknowledge-deliveries" && a.Href == $"/purchasing/purchase-orders/acknowledge?orderNumber={this.order.OrderNumber}");
        }
    }
}
