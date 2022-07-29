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

    public class WhenBatchUpdating : ContextBase
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
            this.key2 = new PurchaseOrderDeliveryKey { OrderNumber = 123457, OrderLine = 1, DeliverySequence = 1 };

            this.changes = new List<PurchaseOrderDeliveryUpdate>
                               {
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key1,
                                           Qty = 100,
                                           UnitPrice = 0.01m,
                                           NewDateAdvised = DateTime.Today
                                       },
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key2,
                                           Qty = 200, 
                                           UnitPrice = 0.01m,
                                           NewDateAdvised = DateTime.Today
                                       }
                               };
            var deliveriesForFirstOrder = new List<PurchaseOrderDelivery>
                                              {
                                                  new PurchaseOrderDelivery
                                                      {
                                                          OrderNumber = this.key1.OrderNumber,
                                                          OrderLine = this.key1.OrderLine,
                                                          DeliverySeq = this.key1.DeliverySequence,
                                                          OurDeliveryQty = 100,
                                                          OrderUnitPriceCurrency = 0.01m
                                                      }
                                              }.AsQueryable();
            var deliveriesForSecondOrder = new List<PurchaseOrderDelivery>
                                               {
                                                   new PurchaseOrderDelivery
                                                       {
                                                           OrderNumber = this.key2.OrderNumber,
                                                           OrderLine = this.key2.OrderLine,
                                                           DeliverySeq = this.key2.DeliverySequence,
                                                           OurDeliveryQty = 200,
                                                           OrderUnitPriceCurrency = 0.01m
                                                       }
                                               }.AsQueryable();
            this.Repository.FilterBy(
                    Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>())
                .Returns(
                    deliveriesForFirstOrder,
                   deliveriesForSecondOrder);
           
            this.RescheduleReasonRepository.FindAll().Returns(new List<RescheduleReason>
                                                                  {
                                                                      new RescheduleReason
                                                                          {
                                                                              Reason = "ADVISED"
                                                                          }
                                                                  }.AsQueryable());

            this.MiniOrderRepository.FindById(this.key1.OrderNumber)
                .Returns(new MiniOrder { OrderNumber = this.key1.OrderNumber });
            this.MiniOrderRepository.FindById(this.key2.OrderNumber)
                .Returns(new MiniOrder { OrderNumber = this.key2.OrderNumber });
            this.MiniOrderDeliveryRepository.FindBy(Arg.Any<Expression<Func<MiniOrderDelivery, bool>>>())
                .Returns(new MiniOrderDelivery { OrderNumber = this.key1.OrderNumber });
            this.result = this.Sut.BatchUpdateDeliveries(this.changes, new List<string>());
        }

        [Test]
        public void ShouldReturnSuccessResult()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("2 records updated successfully.");
            this.result.Errors.Should().BeNullOrEmpty();
        }
    }
}

