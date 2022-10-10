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
                                                              CancelledDetails = new List<CancelledOrderDetail>(),
                                                              Line = 1,
                                                              BaseOurUnitPrice = 1.5m
                                                          }
                                                  }
                               };
            this.PurchaseLedgerPack.GetLedgerPeriod().Returns(12);
            this.PurchaseOrderRepository.FindById(1).Returns(this.current);
            this.MiniOrderRepository.FindById(1).Returns(new MiniOrder());
            this.MockAuthService.HasPermissionFor(
                    AuthorisedAction.PurchaseOrderCancel, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.MockDatabaseService.GetIdSequence("PLOC_SEQ").Returns(123);
            this.OrderReceivedView.FindBy(Arg.Any<Expression<Func<PlOrderReceivedViewEntry, bool>>>())
                .Returns(new PlOrderReceivedViewEntry { QtyOutstanding = 10 });
            this.result = this.Sut.CancelOrder(1, 123, "REASON", new List<string>());
        }

        [Test]
        public void ShouldCancel()
        {
            this.result.Cancelled.Should().Be("Y");
            this.result.Details.All(x => x.Cancelled == "Y").Should().Be(true);
        }

        [Test]
        public void ShouldAddCancelledDetails()
        {
            this.CancelledOrderDetailRepository.Received()
                .Add(Arg.Is<CancelledOrderDetail>(cancelledDetail => 
                    cancelledDetail.Id == 123
                    && cancelledDetail.OrderNumber == this.current.OrderNumber
                    && cancelledDetail.LineNumber == 1
                    && cancelledDetail.PeriodCancelled == 12
                    && cancelledDetail.ReasonCancelled == "REASON"
                    && cancelledDetail.ValueCancelled == 15));
        }
    }
}
