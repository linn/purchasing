namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingOrder : ContextBase
    {
        private PurchaseOrder result;

        private PurchaseOrder current;

        [SetUp]
        public void SetUp()
        {
            this.current = new PurchaseOrder
                               {
                                    Details = new List<PurchaseOrderDetail>
                                                  {
                                                      new PurchaseOrderDetail
                                                          {
                                                              CancelledDetails = new List<CancelledOrderDetail>()
                                                          }
                                                  }
                               };
            this.PurchaseOrderRepository.FindById(1).Returns(this.current);
            this.MiniOrderRepository.FindById(1).Returns(new MiniOrder());
            this.MockAuthService.HasPermissionFor(
                    AuthorisedAction.PurchaseOrderCancel, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.OrderReceivedView.FindBy(Arg.Any<Expression<Func<PlOrderReceivedViewEntry, bool>>>())
                .Returns(new PlOrderReceivedViewEntry());
            this.result = this.Sut.CancelOrder(1, 123, "REASON", new List<string>());
        }

        [Test]
        public void ShouldCancel()
        {
            this.result.Cancelled.Should().Be("Y");
            this.result.Details.All(x => x.Cancelled == "Y").Should().Be(true);
            this.result.Details.All(x => x.CancelledDetails
                .All(c => true)).Should().Be(true);
        }
    }
}
