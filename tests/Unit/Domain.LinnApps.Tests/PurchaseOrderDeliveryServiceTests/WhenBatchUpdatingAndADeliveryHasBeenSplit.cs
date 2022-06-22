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

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBatchUpdatingAndADeliveryHasBeenSplit : ContextBase
    {
        private IEnumerable<PurchaseOrderDeliveryUpdate> changes;

        private PurchaseOrderDeliveryKey key1;

        private PurchaseOrderDeliveryKey key2;

        private BatchUpdateProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });
            this.AuthService
                .HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.key1 = new PurchaseOrderDeliveryKey { OrderNumber = 123456, OrderLine = 1, DeliverySequence = 1 };
            this.key2 = new PurchaseOrderDeliveryKey { OrderNumber = 123456, OrderLine = 1, DeliverySequence = 2 };

            this.changes = new List<PurchaseOrderDeliveryUpdate>
                               {
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key1
                                       }
                               };

            this.Repository.FindById(
                    Arg.Is<PurchaseOrderDeliveryKey>(
                        x => x.OrderLine == this.key1.OrderLine && x.OrderNumber == this.key1.OrderNumber
                                                               && x.DeliverySequence <= 2))
                .Returns(
                    new PurchaseOrderDelivery
                    {
                        OrderNumber = this.key1.OrderNumber,
                        OrderLine = this.key1.OrderLine,
                        DeliverySeq = this.key1.DeliverySequence
                    });
            this.Repository.FilterBy(
                    Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>())
                .Returns(
                    new List<PurchaseOrderDelivery> 
                        { 
                            new PurchaseOrderDelivery
                                {
                                    OrderNumber = this.key1.OrderNumber,
                                    OrderLine = this.key1.OrderLine,
                                    DeliverySeq = this.key1.DeliverySequence
                                },
                                new PurchaseOrderDelivery
                                    {
                                        OrderNumber = this.key2.OrderNumber,
                                        OrderLine = this.key2.OrderLine,
                                        DeliverySeq = this.key2.DeliverySequence
                                    }
                        }.AsQueryable());

            this.result = this.Sut.BatchUpdateDeliveries(this.changes, new List<string>(), true);
        }

        [Test]
        public void ShouldReturnErrorResult()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("0 records updated successfully. The following errors occurred: ");
            this.result.Errors.Count().Should().Be(1);
            this.result.Errors.First().Descriptor.Should().Be(
                $"{this.key1.OrderNumber} / {this.key1.OrderLine} / {this.key1.DeliverySequence}");
            this.result.Errors.First().Message.Should().Be(
                $"{this.key1.OrderNumber} / {this.key1.OrderLine} / {this.key1.DeliverySequence} "
                + $"has been split over multiple deliveries. Please acknowledge manually.");
        }
    }
}
