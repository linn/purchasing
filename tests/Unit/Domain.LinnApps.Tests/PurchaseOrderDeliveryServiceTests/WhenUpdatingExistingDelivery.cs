namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingExistingDelivery : ContextBase
    {
        private PurchaseOrder order;

        private PurchaseOrderDetail line;

        private PurchaseOrderDelivery existingDelivery;

        private IEnumerable<PurchaseOrderDelivery> updatedDeliveries;

        private IEnumerable<PurchaseOrderDelivery> result;

        private decimal vatAmount;

        private decimal baseVatAmount;

        [SetUp]
        public void SetUp()
        {
            this.vatAmount = 543;
            this.baseVatAmount = 544;
            this.existingDelivery = new PurchaseOrderDelivery
                                        {
                                            OrderNumber = 123456,
                                            OurDeliveryQty = 123,
                                            OrderDeliveryQty = 123,
                                            OurUnitPriceCurrency = 555,
                                            DeliverySeq = 1,
                                            OrderLine = 1,
                                        };
            this.updatedDeliveries = new List<PurchaseOrderDelivery>
                                         {
                                             new PurchaseOrderDelivery
                                                 {
                                                     DeliverySeq = 1,
                                                     OurDeliveryQty = 10,
                                                     BaseOurUnitPrice = 11,
                                                     DateRequested = 25.March(2022),
                                                     DateAdvised = 28.March(2022),
                                                     AvailableAtSupplier = "N"
                                                 }
                                         };

            this.line = new PurchaseOrderDetail
                            {
                                Line = 1,
                                OrderUnitPriceCurrency = 666,
                                OrderConversionFactor = 1.5m,
                                BaseOurUnitPrice = 111,
                                PurchaseDeliveries = new List<PurchaseOrderDelivery>
                                                         {
                                                             this.existingDelivery
                                                         },
                                OurQty = 10
                            };
            this.order = new PurchaseOrder
                             {
                                 OrderNumber = 123456,
                                 Details = new List<PurchaseOrderDetail> { this.line },
                                 SupplierId = 9876543,
                                 DocumentTypeName = "PO"
                             };
            this.PurchaseOrderRepository.FindById(this.order.OrderNumber).Returns(this.order);

            this.PurchaseOrdersPack.GetVatAmountSupplier(
                this.line.BaseOurUnitPrice.GetValueOrDefault() * this.updatedDeliveries.First().OurDeliveryQty.GetValueOrDefault(),
                this.order.SupplierId).Returns(this.baseVatAmount);

            this.PurchaseOrdersPack.GetVatAmountSupplier(
                this.line.OrderUnitPriceCurrency.GetValueOrDefault()
                * this.updatedDeliveries.First().OurDeliveryQty.GetValueOrDefault(),
                this.order.SupplierId).Returns(this.vatAmount);

            this.AuthService.HasPermissionFor(
                    AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.DeliveryRepository.FindBy(Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>())
                .Returns(this.existingDelivery);

            this.MiniOrderRepository.FindById(this.order.OrderNumber).Returns(new MiniOrder { });
            this.result = this.Sut.UpdateDeliveriesForOrderLine(
                this.order.OrderNumber,
                this.line.Line,
                this.updatedDeliveries,
                new List<string>());
        }

        [Test]
        public void ShouldReturnResult()
        {
            this.result.Count().Should().Be(1);
            var updateData = this.updatedDeliveries.First();
            var updated = this.result.First();
            updated.OurDeliveryQty.Should().Be(updateData.OurDeliveryQty);
            updated.OrderDeliveryQty.Should().Be(updateData.OurDeliveryQty / this.line.OrderConversionFactor);
            updated.OurUnitPriceCurrency.Should().Be(this.line.OurUnitPriceCurrency);
            updated.DateRequested.Should().Be(updateData.DateRequested);
            updated.DateAdvised.Should().Be(updateData.DateAdvised);
            updated.CallOffDate.Should().BeCloseTo(DateTime.Now, new TimeSpan(0, 1, 0));
            updated.NetTotalCurrency.Should().Be(
                Math.Round(
                    updateData.OurDeliveryQty.GetValueOrDefault() * this.line.OurUnitPriceCurrency.GetValueOrDefault(),
                    2));
            updated.VatTotalCurrency.Should().Be(this.vatAmount);
            updated.DeliveryTotalCurrency.Should().Be(
                Math.Round(
                    this.line.OrderUnitPriceCurrency.GetValueOrDefault() * updated.OurDeliveryQty.GetValueOrDefault(),
                    2) + this.vatAmount);
            updated.BaseOurUnitPrice.Should().Be(updateData.BaseOurUnitPrice);
            updated.BaseOrderUnitPrice.Should().Be(updateData.BaseOrderUnitPrice);
            updated.BaseNetTotal.Should().Be(Math.Round(
                updateData.OurDeliveryQty.GetValueOrDefault() * this.line.BaseOurUnitPrice.GetValueOrDefault(),
                2));
            updated.BaseVatTotal.Should().Be(this.baseVatAmount);
            updated.BaseDeliveryTotal.Should().Be(
                this.baseVatAmount + (updateData.OurDeliveryQty.GetValueOrDefault()
                                      * this.line.BaseOurUnitPrice.GetValueOrDefault()));
            updated.QuantityOutstanding.Should().Be(
                updateData.OurDeliveryQty - this.existingDelivery.OurDeliveryQty
                + this.existingDelivery.QuantityOutstanding);
            updated.RescheduleReason.Should().Be("ADVISED");
            updated.AvailableAtSupplier.Should().Be(updateData.AvailableAtSupplier);
        }

        [Test]
        public void ShouldUpdateMiniOrder()
        {
            this.MiniOrderRepository.Received().FindById(this.order.OrderNumber);
        }
    }
}
