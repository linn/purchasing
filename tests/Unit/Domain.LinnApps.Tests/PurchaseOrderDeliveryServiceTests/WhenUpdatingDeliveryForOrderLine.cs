namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrder;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingDeliveryForOrderLine : ContextBase
    {
        private IEnumerable<PurchaseOrderDelivery> result;

        [SetUp]
        public void SetUp()
        {
            var deliveries = new List<PurchaseOrderDelivery>
                                 {
                                     new PurchaseOrderDelivery
                                         {
                                             OurDeliveryQty = 100,
                                             DeliverySeq = 1,
                                             OrderNumber = 1,
                                             DateAdvised = 28.March(1995)
                                         }
                                 };
            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.MiniOrderRepository.FindById(1).Returns(new MiniOrder());
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
            this.result = this.Sut.UpdateDeliveriesForOrderLine(
                1,
                1,
                deliveries,
                new List<string>());
        }

        [Test]
        public void ShouldReturnUpdated()
        {
            this.result.Count().Should().Be(1);
            this.result.First().OurDeliveryQty.Should().Be(100);
            this.result.First().DateAdvised.Should().Be(28.March(1995));
        }
    }
}
