namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRedistributingDeliveries : ContextBase
    {
        private PurchaseOrder order;

        private PurchaseOrderDetail line;

        private PurchaseOrderDelivery existingDelivery;

        private IEnumerable<PurchaseOrderDelivery> updatedDeliveries;

        private IEnumerable<PurchaseOrderDelivery> result;

        [SetUp]
        public void SetUp()
        {
            this.existingDelivery = new PurchaseOrderDelivery
                                        {
                                            OrderNumber = 123456,
                                            OurDeliveryQty = 500,
                                            OrderDeliveryQty = 500,
                                            OurUnitPriceCurrency = 555,
                                            OrderUnitPriceCurrency = 555,
                                            DeliverySeq = 1,
                                            OrderLine = 1,
                                            QuantityOutstanding = 500
                                        };

            this.updatedDeliveries = new List<PurchaseOrderDelivery>
                                         {
                                             new PurchaseOrderDelivery
                                                 {
                                                     DeliverySeq = 1,
                                                     OurDeliveryQty = 200,
                                                     OrderDeliveryQty = 200,
                                                     BaseOurUnitPrice = 11,
                                                     DateRequested = 25.March(2022),
                                                     DateAdvised = 28.March(2022),
                                                     AvailableAtSupplier = "N"
                                                 },
                                             new PurchaseOrderDelivery
                                                 {
                                                     DeliverySeq = 2,
                                                     OrderDeliveryQty = 200,
                                                     OurDeliveryQty = 200,
                                                     BaseOurUnitPrice = 11,
                                                     DateRequested = 25.March(2022),
                                                     DateAdvised = 28.March(2022),
                                                     AvailableAtSupplier = "N"
                                                 },
                                             new PurchaseOrderDelivery
                                                 {
                                                     DeliverySeq = 3,
                                                     OrderDeliveryQty = 100,
                                                     OurDeliveryQty = 100,
                                                     BaseOurUnitPrice = 11,
                                                     DateRequested = 25.March(2022),
                                                     DateAdvised = 28.March(2022),
                                                     AvailableAtSupplier = "N"
                                                 }
                                         };

            this.line = new PurchaseOrderDetail
            {
                Line = 1,
                OurUnitPriceCurrency = 666m,
                OrderUnitPriceCurrency = 666m,
                OrderConversionFactor = 1m,
                BaseOurUnitPrice = 111,
                PurchaseDeliveries = new List<PurchaseOrderDelivery>
                                                         {
                                                             this.existingDelivery
                                                         },
                OurQty = 500,
                OrderQty = 500
            };
            this.order = new PurchaseOrder
            {
                OrderNumber = 123456,
                Details = new List<PurchaseOrderDetail> { this.line },
                SupplierId = 9876543,
                DocumentTypeName = "PO"
            };
            this.PurchaseOrderRepository.FindById(this.order.OrderNumber).Returns(this.order);

            this.AuthService.HasPermissionFor(
                    AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.PurchaseOrderDeliveryRepository.FindBy(Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>())
                .Returns(this.existingDelivery);

            this.MiniOrderRepository.FindById(this.order.OrderNumber).Returns(new MiniOrder { });
            this.result = this.Sut.UpdateDeliveriesForOrderLine(
                this.order.OrderNumber,
                this.line.Line,
                this.updatedDeliveries,
                new List<string>());
        }

        [Test]
        public void ShouldSetQtyOutstandingCorrectly()
        {
            this.result.Count().Should().Be(3);
            this.result.ElementAt(0).QuantityOutstanding.Should().Be(200);
            this.result.ElementAt(1).QuantityOutstanding.Should().Be(200);
            this.result.ElementAt(2).QuantityOutstanding.Should().Be(100);
        }
    }
}
