namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrdersReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingOrdsByPartReport : ContextBase
    {
        private readonly DateTime orderDate = 5.December(2021);

        private readonly int orderNumber = 9876;

        private readonly string partNumber = "WHO N0z";

        private readonly int supplierId = 77282;

        private readonly string suppliersDesignation = "a sorta description that's quite long total £15millION";

        private ResultsModel results;

        [SetUp]
        public void SetUp()
        {
            var purchaseOrders = new List<PurchaseOrder>
                                     {
                                         new PurchaseOrder
                                             {
                                                 OrderNumber = this.orderNumber,
                                                 SupplierId = this.supplierId,
                                                 Details = new List<PurchaseOrderDetail>
                                                               {
                                                                   new PurchaseOrderDetail
                                                                       {
                                                                           Line = 1,
                                                                           BaseNetTotal = 14.12m,
                                                                           NetTotalCurrency = 17.23m,
                                                                           OrderNumber = this.orderNumber,
                                                                           OurQty = 3,
                                                                           Part = new Part { PartNumber = this.partNumber },
                                                                           PurchaseDeliveries =
                                                                               new List<PurchaseOrderDelivery>
                                                                                   {
                                                                                       new PurchaseOrderDelivery
                                                                                           {
                                                                                               QtyNetReceived = 11,
                                                                                               DeliverySeq = 2,
                                                                                               OurDeliveryQty = 13,
                                                                                               NetTotalCurrency = 2m,
                                                                                               DateRequested =
                                                                                                   11.November(2021),
                                                                                               DateAdvised =
                                                                                                   11.December(2021)
                                                                                           },
                                                                                       new PurchaseOrderDelivery
                                                                                           {
                                                                                               QtyNetReceived = 11,
                                                                                               DeliverySeq = 3,
                                                                                               OurDeliveryQty = 13,
                                                                                               NetTotalCurrency = 2m,
                                                                                               DateRequested =
                                                                                                   19.November(2021),
                                                                                               DateAdvised =
                                                                                                   19.December(2021)
                                                                                           }
                                                                                   },
                                                                           SuppliersDesignation =
                                                                               this.suppliersDesignation
                                                                       }
                                                               },
                                                 DocumentTypeName = "Suhn",
                                                 Currency = new Currency { Code = "USD" },
                                                 Supplier = new Supplier
                                                                {
                                                                    Name = "We sell stuff", SupplierId = this.supplierId
                                                                },
                                                 OrderDate = this.orderDate
                                             }
                                     }.AsQueryable();
            this.PurchaseOrderRepository.FilterBy(Arg.Any<Expression<Func<PurchaseOrder, bool>>>())
                .Returns(purchaseOrders);

            var purchaseLedgers = new List<PurchaseLedger>
                                      {
                                          new PurchaseLedger
                                              {
                                                  OrderNumber = this.orderNumber,
                                                  OrderLine = 1,
                                                  PlTransType = "Suhn",
                                                  TransactionType =
                                                      new TransactionType { DebitOrCredit = "C", TransType = "Suhn" },
                                                  PlQuantity = 77
                                              }
                                      }.AsQueryable();

            this.results = this.Sut.GetOrdersByPartReport(1.October(2021), 5.December(2021), this.partNumber, false);
        }

        [Test]
        public void ShouldCallRepos()
        {
            this.PurchaseOrderRepository.Received().FilterBy(Arg.Any<Expression<Func<PurchaseOrder, bool>>>());
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.ReportTitle.DisplayValue.Should().Be($"Purchase Orders By Part: {this.partNumber}");
            this.results.Rows.Count().Should().Be(2);
            var row = this.results.Rows.First();
            row.RowId.Should().Be($"{this.orderNumber}/1/2");
            this.results.GetGridTextValue(0, 0).Should().Be($"{this.orderNumber}/Line1/Delivery2");
            this.results.GetGridTextValue(0, 1).Should().Be(this.orderDate.ToString("dd-MMM-yyyy"));
            this.results.GetGridTextValue(0, 2).Should().Be($"{this.supplierId}: We sell stuff");
            this.results.GetGridValue(0, 3).Should().Be(3);
            this.results.GetGridValue(0, 4).Should().Be(11);
            this.results.GetGridValue(0, 5).Should().Be(14.12m);
            this.results.GetGridTextValue(0, 6).Should().Be("USD");
            this.results.GetGridValue(0, 7).Should().Be(17.23m);
            this.results.GetGridTextValue(0, 8).Should().Be("2");
            this.results.GetGridValue(0, 9).Should().Be(13);
            this.results.GetGridTextValue(1, 0).Should().Be($"{this.orderNumber}/Line1/Delivery3");
            this.results.GetGridTextValue(1, 8).Should().Be("3");
        }

        [Test]
        public void ShouldOnlyPopulateTotalsColumnsForFirstDelivery()
        {
            this.results.GetGridValue(0, 3).Should().Be(3);
            this.results.GetGridValue(1, 3).Should().BeNull();

            this.results.GetGridValue(0, 4).Should().Be(11);
            this.results.GetGridValue(1, 4).Should().Be(11);

            this.results.GetGridValue(0, 5).Should().Be(14.12m);
            this.results.GetGridValue(1, 5).Should().BeNull();

            this.results.GetGridTextValue(0, 6).Should().Be("USD");
            this.results.GetGridTextValue(1, 6).Should().Be("USD");

            this.results.GetGridValue(0, 7).Should().Be(17.23m);
            this.results.GetGridValue(1, 7).Should().BeNull();
        }
    }
}
