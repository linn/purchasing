namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBatchUpdatingAndQtyMismatch : ContextBase
    {
        private IEnumerable<PurchaseOrderDeliveryUpdate> changes;

        private PurchaseOrderDeliveryKey key1;

        private BatchUpdateProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.AuthService
                .HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.key1 = new PurchaseOrderDeliveryKey { OrderNumber = 123456, OrderLine = 1, DeliverySequence = 1 };

            this.changes = new List<PurchaseOrderDeliveryUpdate>
                               {
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key1,
                                           Qty = 100
                                       }
                               };

            this.Repository.FilterBy(
                    Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>())
                .Returns(
                    new List<PurchaseOrderDelivery>
                        {
                            new PurchaseOrderDelivery
                                {
                                    OrderNumber = this.key1.OrderNumber,
                                    OrderLine = this.key1.OrderLine,
                                    DeliverySeq = this.key1.DeliverySequence,
                                    OurDeliveryQty = 600
                                }
                        }.AsQueryable());
           
            this.MiniOrderRepository.FindById(this.key1.OrderNumber)
                .Returns(new MiniOrder { OrderNumber = this.key1.OrderNumber });
            this.MiniOrderDeliveryRepository.FindBy(Arg.Any<Expression<Func<MiniOrderDelivery, bool>>>())
                .Returns(new MiniOrderDelivery { OrderNumber = this.key1.OrderNumber });
            this.result = this.Sut.BatchUpdateDeliveries(this.changes, new List<string>());
        }

        [Test]
        public void ShouldReturnErrorResult()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("0 records updated successfully. The following errors occurred: ");
            this.result.Errors.Count().Should().Be(1);
            this.result.Errors.First().Descriptor.Should().Be(
                $"Order: {this.key1.OrderNumber}");
            this.result.Errors.First().Message.Should().Be(
                "Qty on lines uploaded for the specified order does not match qties on the corresponding delivery on our system");
        }
    }
}
