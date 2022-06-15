namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingDetailsAndQuantityMismatch : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var deliveries = new List<PurchaseOrderDelivery> 
                                 { 
                                     new PurchaseOrderDelivery
                                         {
                                            OurDeliveryQty = 200
                                         }
                                 };
            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.PurchaseOrderRepository.FindById(1).Returns(new PurchaseOrder
            {
                OrderNumber = 1,
                DocumentTypeName = "PO",
                OrderMethod = new OrderMethod
                                  {
                                      Name = "METHOD"
                                  },
                Details = new List<PurchaseOrderDetail>
                              {
                                  new PurchaseOrderDetail
                                      {
                                          Line = 1,
                                          OurQty = 100,
                                          PurchaseDeliveries = deliveries
                                      }
                              }
            });
            this.action = () => this.Sut.UpdateDeliveriesForOrderLine(
                1,
                1,
                deliveries,
                new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PurchaseOrderDeliveryException>()
                .WithMessage("You must match the order qty when splitting deliveries.");
        }
    }
}
