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

    public class WhenBatchUploading : ContextBase
    {
        private IEnumerable<PurchaseOrderDeliveryUpdate> changes;

        private PurchaseOrderDeliveryKey key1;

        private PurchaseOrderDeliveryKey key12;

        private PurchaseOrderDeliveryKey key13;

        private PurchaseOrderDeliveryKey key2;

        private BatchUpdateProcessResult result;

        private PurchaseOrder order1;

        private PurchaseOrder order2;

        [SetUp]
        public void SetUp()
        {
            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });
            this.order1 = new PurchaseOrder
                             {
                                 OrderNumber = 123456,
                                 SupplierId = 123,
                                 Details = new List<PurchaseOrderDetail>()
                             };
            this.order2 = new PurchaseOrder
                              {
                                  OrderNumber = 123457,
                                  SupplierId = 1234,
                                  Details = new List<PurchaseOrderDetail>()
                              };
            this.order1.Details.Add(new PurchaseOrderDetail { PurchaseOrder = this.order1 });
            this.order2.Details.Add(new PurchaseOrderDetail { PurchaseOrder = this.order2 });

            this.AuthService
                .HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.key1 = new PurchaseOrderDeliveryKey { OrderNumber = 123456, OrderLine = 1 };
            this.key12 = new PurchaseOrderDeliveryKey { OrderNumber = 123456, OrderLine = 1 };
            this.key13 = new PurchaseOrderDeliveryKey { OrderNumber = 123456, OrderLine = 1 };

            this.key2 = new PurchaseOrderDeliveryKey { OrderNumber = 123457, OrderLine = 1 };

            this.changes = new List<PurchaseOrderDeliveryUpdate>
                               {
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key1,
                                           Qty = 100,
                                           UnitPrice = 0.03m,
                                           NewDateAdvised = DateTime.Today
                                       },
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key12,
                                           Qty = 200,
                                           UnitPrice = 0.03m,
                                           NewDateAdvised = DateTime.Today
                                       },
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key13,
                                           Qty = 300,
                                           UnitPrice = 0.03m,
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
                                                          OurDeliveryQty = 100,
                                                          OrderUnitPriceCurrency = 0.03m,
                                                          PurchaseOrderDetail = this.order1.Details.First()
                                                      },
                                                  new PurchaseOrderDelivery
                                                      {
                                                          OrderNumber = this.key1.OrderNumber,
                                                          OrderLine = this.key1.OrderLine,
                                                          OurDeliveryQty = 200,
                                                          OrderUnitPriceCurrency = 0.03m,
                                                          PurchaseOrderDetail = this.order1.Details.First()
                                                      },
                                                  new PurchaseOrderDelivery
                                                      {
                                                          OrderNumber = this.key1.OrderNumber,
                                                          OrderLine = this.key1.OrderLine,
                                                          OurDeliveryQty = 300,
                                                          OrderUnitPriceCurrency = 0.03m,
                                                          PurchaseOrderDetail = this.order1.Details.First()
                                                      }
                                              }.AsQueryable();
            var deliveriesForSecondOrder = new List<PurchaseOrderDelivery>
                                               {
                                                   new PurchaseOrderDelivery
                                                       {
                                                           OrderNumber = this.key2.OrderNumber,
                                                           OrderLine = this.key2.OrderLine,
                                                           OurDeliveryQty = 200,
                                                           OrderUnitPriceCurrency = 0.01m,
                                                           PurchaseOrderDetail = this.order2.Details.First()
                                                       }
                                               }.AsQueryable();

            this.PurchaseOrderRepository.FindById(this.key1.OrderNumber)
                .Returns(new PurchaseOrder
                             {
                                 Details = new List<PurchaseOrderDetail>
                                               {
                                                   new PurchaseOrderDetail
                                                       {
                                                           OrderQty = 600, Line = 1, OrderUnitPriceCurrency = 0.03m
                                                       }
                                               }
                             });
            this.PurchaseOrderRepository.FindById(this.key2.OrderNumber)
                .Returns(new PurchaseOrder
                             {
                                 Details = new List<PurchaseOrderDetail>
                                               {
                                                   new PurchaseOrderDetail
                                                       {
                                                           OrderQty = 200, Line = 1, OrderUnitPriceCurrency = 0.01m
                                                       }
                                               }
                             });

            this.Repository.FilterBy(
                    Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>())
                .Returns(
                    deliveriesForFirstOrder,
                   deliveriesForSecondOrder);

            this.Repository.FindBy(Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>()).Returns(
                deliveriesForFirstOrder.ElementAt(0),
                deliveriesForFirstOrder.ElementAt(1),
                deliveriesForFirstOrder.ElementAt(2),
                deliveriesForSecondOrder.ElementAt(0));
            
            this.MiniOrderRepository.FindById(this.key1.OrderNumber)
                .Returns(new MiniOrder { OrderNumber = this.key1.OrderNumber });
            this.MiniOrderRepository.FindById(this.key2.OrderNumber)
                .Returns(new MiniOrder { OrderNumber = this.key2.OrderNumber });
            this.MiniOrderDeliveryRepository.FindBy(Arg.Any<Expression<Func<MiniOrderDelivery, bool>>>())
                .Returns(new MiniOrderDelivery { OrderNumber = this.key1.OrderNumber });
            this.result = this.Sut.UploadDeliveries(this.changes, new List<string>());
        }

        [Test]
        public void ShouldReturnSuccessResult()
        {
            this.result.Success.Should().BeTrue();
            this.result.Message.Should().Be("2 orders updated successfully.");
            this.result.Errors.Should().BeNullOrEmpty();
        }
    }
}

