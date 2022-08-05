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

    public class WhenBatchUpdatingAndDuplicateDeliverySequenceLinesForOrder : ContextBase 
    {
        private IEnumerable<PurchaseOrderDeliveryUpdate> changes;

        private PurchaseOrderDeliveryKey key11;

        private PurchaseOrderDeliveryKey key12;

        private BatchUpdateProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.AuthService
                .HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.key11 = new PurchaseOrderDeliveryKey { OrderNumber = 123456, OrderLine = 1, DeliverySequence = 1 };

            this.key12 = new PurchaseOrderDeliveryKey { OrderNumber = 123456, OrderLine = 1, DeliverySequence = 1 };

            this.changes = new List<PurchaseOrderDeliveryUpdate>
                               {
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key11,
                                           Qty = 100,
                                           UnitPrice = 0.05m,
                                           NewDateAdvised = DateTime.Now
                                       },
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key12,
                                           Qty = 100,
                                           UnitPrice = 0.05m,
                                           NewDateAdvised = DateTime.Now
                                       }
                               };

            this.Repository.FilterBy(
                    Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>())
                .Returns(
                    new List<PurchaseOrderDelivery>
                        {
                            new PurchaseOrderDelivery
                                {
                                    OrderNumber = this.key11.OrderNumber,
                                    OrderLine = this.key11.OrderLine,
                                    DeliverySeq = this.key11.DeliverySequence,
                                    OurDeliveryQty = 100,
                                    OrderUnitPriceCurrency = 0.05m
                                },
                            new PurchaseOrderDelivery
                                {
                                    OrderNumber = this.key12.OrderNumber,
                                    OrderLine = this.key12.OrderLine,
                                    DeliverySeq = this.key12.DeliverySequence,
                                    OurDeliveryQty = 100,
                                    OrderUnitPriceCurrency = 0.05m
                                }
                        }.AsQueryable());

            this.MiniOrderRepository.FindById(this.key11.OrderNumber)
                .Returns(new MiniOrder { OrderNumber = this.key11.OrderNumber });
            this.MiniOrderDeliveryRepository.FindBy(Arg.Any<Expression<Func<MiniOrderDelivery, bool>>>())
                .Returns(new MiniOrderDelivery { OrderNumber = this.key11.OrderNumber });
            this.result = this.Sut.BatchUpdateDeliveries(this.changes, new List<string>());
        }

        [Test]
        public void ShouldReturnErrorResult()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("0 records updated successfully. The following errors occurred: ");
            this.result.Errors.Count().Should().Be(1);
            this.result.Errors.First().Descriptor.Should().Be(
                $"Order: {this.key11.OrderNumber}");
            this.result.Errors.First().Message.Should().Be(
                "Duplicate delivery sequence entries for the specified order.");
        }
    }
}
