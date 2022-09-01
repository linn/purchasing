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

    public class WhenBatchUploadingAndPriceMismatch : ContextBase
    {
        private IEnumerable<PurchaseOrderDeliveryUpdate> changes;

        private PurchaseOrderDeliveryKey key;

        private BatchUpdateProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.AuthService
                .HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.key = new PurchaseOrderDeliveryKey { OrderNumber = 123456, OrderLine = 1, DeliverySequence = 1 };

            this.changes = new List<PurchaseOrderDeliveryUpdate>
                               {
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key,
                                           Qty = 100,
                                           UnitPrice = 0.05m,
                                           NewDateAdvised = DateTime.Now
                                       }
                               };

            this.PurchaseOrderRepository.FindById(this.key.OrderNumber)
                .Returns(new PurchaseOrder
                             {
                                 Details = new List<PurchaseOrderDetail>
                                               {
                                                   new PurchaseOrderDetail
                                                       {
                                                           OrderQty = 100, Line = 1, OrderUnitPriceCurrency = 0.04m
                                                       }
                                               }
                             });

            this.Repository.FilterBy(
                    Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>())
                .Returns(
                    new List<PurchaseOrderDelivery>
                        {
                            new PurchaseOrderDelivery
                                {
                                    OrderNumber = this.key.OrderNumber,
                                    OrderLine = this.key.OrderLine,
                                    DeliverySeq = this.key.DeliverySequence,
                                    OurDeliveryQty = 100,
                                    OrderUnitPriceCurrency = 0.04m
                                }
                        }.AsQueryable());
            
            this.MiniOrderRepository.FindById(this.key.OrderNumber)
                .Returns(new MiniOrder { OrderNumber = this.key.OrderNumber });
            this.MiniOrderDeliveryRepository.FindBy(Arg.Any<Expression<Func<MiniOrderDelivery, bool>>>())
                .Returns(new MiniOrderDelivery { OrderNumber = this.key.OrderNumber });
            this.result = this.Sut.UploadDeliveries(this.changes, new List<string>());
        }

        [Test]
        public void ShouldReturnErrorResult()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("0 orders updated successfully. The following errors occurred: ");
            this.result.Errors.Count().Should().Be(1);
            this.result.Errors.First().Descriptor.Should().Be(
                $"Order: {this.key.OrderNumber}");
            this.result.Errors.First().Message.Should().Be(
                "Unit Price on lines uploaded (0.05) for the specified order does not match unit price on our system (0.04)");
        }
    }
}
