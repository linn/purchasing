namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Finance.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenFilCancellingOrderLine : ContextBase
    {
        private PurchaseOrder result;

        private PurchaseOrder order;

        private int orderNumber;

        private MiniOrder miniOrder;

        private int cancelId;

        private int cancelledBy;

        [SetUp]
        public void SetUp()
        {
            this.cancelId = 123;
            this.cancelledBy = 456;
            this.orderNumber = 808808;
            this.order = new PurchaseOrder
                               {
                                   OrderNumber = this.orderNumber,
                                   FilCancelled = "N",
                                   Details = new List<PurchaseOrderDetail>
                                                 {
                                                     new PurchaseOrderDetail
                                                         {
                                                             Line = 1,
                                                             CancelledDetails = new List<CancelledOrderDetail>()
                                                         },
                                                     new PurchaseOrderDetail
                                                         {
                                                             Line = 2,
                                                             CancelledDetails = new List<CancelledOrderDetail>()
                                                         }
                                                 }
                               };
            this.miniOrder = new MiniOrder { OrderNumber = this.orderNumber };

            this.PurchaseLedgerPack.GetLedgerPeriod().Returns(123);
            this.MiniOrderRepository.FindById(this.orderNumber).Returns(this.miniOrder);
            this.PurchaseOrderRepository.FindById(this.orderNumber).Returns(this.order);
            this.MockAuthService.HasPermissionFor(
                    AuthorisedAction.PurchaseOrderFilCancel, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.MockDatabaseService.GetIdSequence("PLOC_SEQ").Returns(this.cancelId);
            this.ImmediateLiabilityBaseRepository.FindBy(Arg.Any<Expression<Func<ImmediateLiabilityBase, bool>>>())
                .Returns(new ImmediateLiabilityBase { Liability = 10.10m });
            this.ImmediateLiabilityRepository.FindBy(Arg.Any<Expression<Func<ImmediateLiability, bool>>>())
                .Returns(new ImmediateLiability { Liability = 20.20m });
            this.result = this.Sut.FilCancelLine(
                this.orderNumber,
                2,
                this.cancelledBy,
                "some reason",
                new List<string>());
        }

        [Test]
        public void ShouldFilCancelTheLine()
        {
            this.result.Details.First(a => a.Line == 2).FilCancelled.Should().Be("Y");
        }

        [Test]
        public void ShouldAddCancelledDetails()
        {
            this.CancelledOrderDetailRepository.Received()
                .Add(Arg.Is<CancelledOrderDetail>(cancelledDetail => 
                    cancelledDetail.Id == this.cancelId
                    && cancelledDetail.OrderNumber == this.orderNumber
                    && cancelledDetail.LineNumber == 2
                    && cancelledDetail.PeriodFilCancelled == 123
                    && cancelledDetail.FilCancelledById == this.cancelledBy
                    && cancelledDetail.ReasonFilCancelled == "some reason"
                    && cancelledDetail.BaseValueFilCancelled == 10.10m
                    && cancelledDetail.ValueFilCancelled == 20.20m
                    && cancelledDetail.DateFilCancelled == DateTime.Today));
        }

        [Test]
        public void ShouldUpdateMiniOrder()
        {
            this.miniOrder.FilCancelledBy.Should().Be(this.cancelledBy);
            this.miniOrder.DateFilCancelled.Should().HaveValue();
            this.miniOrder.ReasonFilCancelled.Should().Be("some reason");
        }

        [Test]
        public void ShouldNotFilCancelTopLevelOrder()
        {
            this.result.FilCancelled.Should().Be("N");
        }
    }
}
