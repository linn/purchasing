namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Finance.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUnFilCancellingOrderLine : ContextBase
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
                                   FilCancelled = "Y",
                                   DateFilCancelled = 1.July(2025),
                                   PeriodFilCancelled = 123,
                                   Details = new List<PurchaseOrderDetail>
                                                 {
                                                     new PurchaseOrderDetail
                                                         {
                                                             Line = 1,
                                                             FilCancelled = "Y",
                                                             PeriodFilCancelled = 123,
                                                             DateFilCancelled = 1.July(2025),
                                                             CancelledDetails = new List<CancelledOrderDetail>
                                                                 {
                                                                     new CancelledOrderDetail
                                                                         {
                                                                             FilCancelledById = 808,
                                                                             DateFilCancelled = 1.July(2025),
                                                                             ReasonFilCancelled = "some reason",
                                                                             PeriodFilCancelled = 123,
                                                                             BaseValueFilCancelled = 10.10m,
                                                                             ValueFilCancelled = 20.20m
                                                                         }
                                                                 }
                                                         },
                                                     new PurchaseOrderDetail
                                                         {
                                                             Line = 2,
                                                             FilCancelled = "Y",
                                                             PeriodFilCancelled = 122,
                                                             DateFilCancelled = 1.June(2025),
                                                             CancelledDetails = new List<CancelledOrderDetail>
                                                                 {
                                                                     new CancelledOrderDetail
                                                                         {
                                                                             FilCancelledById = 808,
                                                                             DateFilCancelled = 1.June(2025),
                                                                             ReasonFilCancelled = "some other reason",
                                                                             PeriodFilCancelled = 122,
                                                                             BaseValueFilCancelled = 10m,
                                                                             ValueFilCancelled = 20m
                                                                         }
                                                                 }
                                                         }
                                                 },
                               };
            this.miniOrder = new MiniOrder
                                 {
                                     OrderNumber = this.orderNumber,
                                     FilCancelledBy = 123,
                                     DateFilCancelled = 1.July(2025),
                                     ReasonFilCancelled = "some reason"
                                 };

            this.MiniOrderRepository.FindById(this.orderNumber).Returns(this.miniOrder);
            this.PurchaseOrderRepository.FindById(this.orderNumber).Returns(this.order);
            this.MockAuthService.HasPermissionFor(
                    AuthorisedAction.PurchaseOrderFilCancel, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.result = this.Sut.UnFilCancelLine(
                this.orderNumber,
                1,
                new List<string>());
        }

        [Test]
        public void ShouldUnFilCancelTheLine()
        {
            var detail = this.result.Details.First(a => a.Line == 1);
            detail.FilCancelled.Should().Be("N");
            detail.DateFilCancelled.Should().BeNull();
            detail.PeriodFilCancelled.Should().BeNull();
        }

        [Test]
        public void ShouldUpdateCancelledDetails()
        {
            var cancelledDetails = this.order.Details.First(a => a.Line == 1).CancelledDetails.First();
            cancelledDetails.DateFilUncancelled.Should().Be(DateTime.Today);
        }

        [Test]
        public void ShouldUpdateMiniOrder()
        {
            this.miniOrder.FilCancelledBy.Should().BeNull();
            this.miniOrder.DateFilCancelled.Should().BeNull();
            this.miniOrder.ReasonFilCancelled.Should().BeNull();
        }

        [Test]
        public void ShouldUpdateTopLevelOrder()
        {
            this.result.FilCancelled.Should().Be("N");
            this.result.DateFilCancelled.Should().BeNull();
            this.result.PeriodFilCancelled.Should().BeNull();
        }
    }
}
