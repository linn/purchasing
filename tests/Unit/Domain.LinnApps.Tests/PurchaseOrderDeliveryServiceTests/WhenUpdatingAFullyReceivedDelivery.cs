namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAFullyReceivedDelivery
        : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var existingDelivery = new PurchaseOrderDelivery
                                        {
                                            OrderNumber = 123456,
                                            OurDeliveryQty = 123,
                                            OrderDeliveryQty = 123,
                                            OurUnitPriceCurrency = 555,
                                            DeliverySeq = 1,
                                            OrderLine = 1,
                                            QuantityOutstanding = 0
                                        };
            var updatedDeliveries = new List<PurchaseOrderDelivery>
                                         {
                                             new PurchaseOrderDelivery
                                                 {
                                                     DeliverySeq = 1,
                                                     OrderDeliveryQty = 10,
                                                     OurDeliveryQty = 10,
                                                     BaseOurUnitPrice = 11,
                                                     DateRequested = 25.March(2022),
                                                     DateAdvised = 28.March(2022),
                                                     AvailableAtSupplier = "N"
                                                 }
                                         };

            var line = new PurchaseOrderDetail
                            {
                                Line = 1,
                                OrderUnitPriceCurrency = 666,
                                OurUnitPriceCurrency = 666,
                                OrderConversionFactor = 1m,
                                BaseOurUnitPrice = 111,
                                PurchaseDeliveries = new List<PurchaseOrderDelivery>
                                                         {
                                                             existingDelivery
                                                         },
                                OrderQty = 10m,
                                OurQty = 10
                            };
            var order = new PurchaseOrder
                             {
                                 OrderNumber = 123456,
                                 Details = new List<PurchaseOrderDetail> { line },
                                 SupplierId = 9876543,
                                 DocumentTypeName = "PO"
                             };
            this.PurchaseOrderRepository.FindById(order.OrderNumber).Returns(order);

            this.action = () => this.Sut.UpdateDeliveriesForOrderLine(
                123456,
                1,
                updatedDeliveries,
                new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<PurchaseOrderDeliveryException>()
                .WithMessage("Cannot change qty for a delivery that has been received - DEL: 1 QTY: 123");
        }
    }
}