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

    public class WhenFilCancellingOnlyOrderLine : ContextBase
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
                1,
                this.cancelledBy,
                "some reason",
                new List<string>());
        }

        [Test]
        public void ShouldFilCancelTheLine()
        {
            this.result.Details.First(a => a.Line == 1).FilCancelled.Should().Be("Y");
        }

        [Test]
        public void ShouldFilCancelTopLevelOrder()
        {
            this.result.FilCancelled.Should().Be("Y");
        }
    }
}
