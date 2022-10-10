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

    public class WhenUnCancellingOrder : ContextBase
    {
        private PurchaseOrder result;

        private PurchaseOrder current;

        [SetUp]
        public void SetUp()
        {
            this.current = new PurchaseOrder
            {
                Details = 
                    new List<PurchaseOrderDetail>
                        {
                            new PurchaseOrderDetail
                                {
                                    CancelledDetails = new List<CancelledOrderDetail>
                                                           {
                                                               new CancelledOrderDetail
                                                                   {
                                                                       DateCancelled = DateTime.UnixEpoch,
                                                                       ValueCancelled = 1000m
                                                                   }
                                                           },
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
            this.MockDatabaseService.GetNextVal("PLOC_SEQ").Returns(123);
            this.OrderReceivedView.FindBy(Arg.Any<Expression<Func<PlOrderReceivedViewEntry, bool>>>())
                .Returns(new PlOrderReceivedViewEntry { QtyOutstanding = 10 });
            this.result = this.Sut.UnCancelOrder(1, new List<string>());
        }

        [Test]
        public void ShouldCancel()
        {
            this.result.Cancelled.Should().Be("N");
            this.result.Details.All(x => x.Cancelled == "N").Should().Be(true);
        }

        [Test]
        public void ShouldUpdateCancelledDetails()
        {
            this.result.Details.All(
                x => 
                    x.CancelledDetails.All(
                        c => c.ValueCancelled == 0 
                             && c.DateUncancelled == DateTime.Today))
                .Should().Be(true);
        }
    }
}
