namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests.SwitchQtyPriceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NUnit.Framework;

    public class WhenSwitchingButAlreadyReceived : SwitchQtyPriceContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.PurchaseOrder.Details = new List<PurchaseOrderDetail>
                                             {
                                                 new PurchaseOrderDetail
                                                     {
                                                         OrderNumber = this.OrderNumber,
                                                         Line = 1,
                                                         Part = new Part { PartNumber = "P1", StockControlled = "N" },
                                                         PartNumber = "P1",
                                                         OrderQty = 1,
                                                         OurQty = 1,
                                                         OurUnitPriceCurrency = 3800,
                                                         OrderUnitPriceCurrency = 3800,
                                                         OrderConversionFactor = 1,
                                                         BaseOurUnitPrice = 3800,
                                                         BaseOrderUnitPrice = 3800,
                                                         DetailTotalCurrency = 3800,
                                                         NetTotalCurrency = 3800,
                                                         PurchaseDeliveries = new List<PurchaseOrderDelivery>
                                                                                  {
                                                                                      new PurchaseOrderDelivery
                                                                                          {
                                                                                              OurDeliveryQty = 1,
                                                                                              OrderDeliveryQty = 1,
                                                                                              OurUnitPriceCurrency =
                                                                                                  3800,
                                                                                              OrderUnitPriceCurrency =
                                                                                                  3800,
                                                                                              BaseOurUnitPrice = 3800,
                                                                                              BaseOrderUnitPrice = 3800,
                                                                                              NetTotalCurrency = 3800,
                                                                                              DeliveryTotalCurrency =
                                                                                                  3800,
                                                                                              QtyNetReceived = 1
                                                                                          }
                                                                                  },
                                                         OrderPosting = new PurchaseOrderPosting { Qty = 1 }
                                                     }
                                             };

            this.Action = () => this.Sut.SwitchOurQtyAndPrice(this.OrderNumber, 1, this.EmployeeId, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.Action.Should().Throw<InvalidActionException>()
                .WithMessage($"Cannot switch qty and price on order {this.OrderNumber}/1 because deliveries have been received.");
        }
    }
}
