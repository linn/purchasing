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

    public class WhenBatchUploadingAndPricesMatchTo4DecimalPlaces : ContextBase
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
                                           UnitPrice = 0.01112m,
                                           NewDateAdvised = DateTime.Today
                                       }
                               };

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
                                    OrderUnitPriceCurrency = 0.0111m,
                                    QtyNetReceived = 0
                                }
                        }.AsQueryable());

            this.PurchaseOrderRepository.FindById(this.key.OrderNumber)
                .Returns(new PurchaseOrder
                             {
                                 Details = new List<PurchaseOrderDetail>
                                               {
                                                   new PurchaseOrderDetail { OrderQty = 100, Line = 1, OrderUnitPriceCurrency = 0.01111m }
                                               }
                             });

            this.MiniOrderRepository.FindById(this.key.OrderNumber)
                .Returns(new MiniOrder { OrderNumber = this.key.OrderNumber });
            this.MiniOrderDeliveryRepository.FindBy(Arg.Any<Expression<Func<MiniOrderDelivery, bool>>>())
                .Returns(new MiniOrderDelivery { OrderNumber = this.key.OrderNumber });
            this.result = this.Sut.UploadDeliveries(this.changes, new List<string>());
        }

        [Test]
        public void ShouldReturnSuccessResult()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("1 orders updated successfully.");
            this.result.Errors.Should().BeNullOrEmpty();
        }
    }
}
