﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrdersReportServiceTests
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

    public class WhenGettingOrdsBySupplierReport : ContextBase
    {
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
                                                                           BaseNetTotal = 14.88m,
                                                                           NetTotalCurrency = 17.34m,
                                                                           OrderNumber = this.orderNumber,
                                                                           OurQty = 3,
                                                                           Part = new Part { PartNumber = this.partNumber },
                                                                           PurchaseDeliveries =
                                                                               new List<PurchaseOrderDelivery>
                                                                                   {
                                                                                        new PurchaseOrderDelivery
                                                                                           {
                                                                                               QtyNetReceived = 11,
                                                                                               DeliverySeq = 1,
                                                                                               OurDeliveryQty = 13,
                                                                                               NetTotalCurrency = 2m,
                                                                                               DateRequested =
                                                                                                   1.November(2021),
                                                                                               DateAdvised =
                                                                                                   1.December(2021)
                                                                                           },
                                                                                       new PurchaseOrderDelivery
                                                                                           {
                                                                                               QtyNetReceived = 11,
                                                                                               DeliverySeq = 2,
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

            this.PurchaseLedgerRepository.FilterBy(Arg.Any<Expression<Func<PurchaseLedger, bool>>>())
                .Returns(purchaseLedgers);

            this.SupplierRepository.FindById(Arg.Any<int>())
                .Returns(new Supplier { Name = "We sell stuff", SupplierId = this.supplierId });

            this.results = this.Sut.GetOrdersBySupplierReport(
                new DateTime(2021, 10, 1),
                new DateTime(2021, 12, 5),
                this.supplierId,
                true,
                false,
                true,
                "Y",
                "A");
        }

        [Test]
        public void ShouldCallRepos()
        {
            this.PurchaseOrderRepository.Received().FilterBy(Arg.Any<Expression<Func<PurchaseOrder, bool>>>());
            this.PurchaseLedgerRepository.Received().FilterBy(Arg.Any<Expression<Func<PurchaseLedger, bool>>>());
            this.SupplierRepository.Received().FindById(Arg.Any<int>());
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.ReportTitle.DisplayValue.Should()
                .Be($"Purchase Orders By Supplier - {this.supplierId}: We sell stuff");
            this.results.Rows.Count().Should().Be(2);
            var row = this.results.Rows.First();
            row.RowId.Should().Be($"{this.orderNumber}/1/1");
            this.results.GetGridTextValue(0, 0).Should().Be($"{this.orderNumber}");
            this.results.GetGridTextValue(0, 1).Should().Be("1");
            this.results.GetGridTextValue(0, 2).Should().Be(this.partNumber);
            this.results.GetGridTextValue(0, 3).Should().Be(this.suppliersDesignation);
            this.results.GetGridTextValue(0, 4).Should().Be("3");
            this.results.GetGridTextValue(0, 5).Should().Be("22");
            this.results.GetGridTextValue(0, 6).Should().Be("77");
            this.results.GetGridValue(0, 7).Should().Be(14.88m);
            this.results.GetGridTextValue(0, 8).Should().Be("USD");
            this.results.GetGridValue(0, 9).Should().Be(17.34m);
            this.results.GetGridTextValue(0, 10).Should().Be("1");
            this.results.GetGridTextValue(0, 11).Should().Be("13");
            this.results.GetGridTextValue(0, 12).Should().Be("01-Nov-2021");
            this.results.GetGridTextValue(0, 13).Should().Be("01-Dec-2021");
            this.results.GetGridTextValue(1, 10).Should().Be("2");
        }

        [Test]
        public void ShouldOnlyPopulateTotalsColumnsForFirstDelivery()
        {
            this.results.GetGridTextValue(0, 4).Should().Be("3");
            this.results.GetGridTextValue(1, 5).Should().BeNull();

            this.results.GetGridTextValue(0, 5).Should().Be("22");

            this.results.GetGridTextValue(0, 6).Should().Be("77");
            this.results.GetGridTextValue(1, 6).Should().BeNullOrEmpty();

            this.results.GetGridValue(0, 7).Should().Be(14.88m);
            this.results.GetGridValue(1, 7).Should().BeNull();

            this.results.GetGridTextValue(0, 8).Should().Be("USD");
            this.results.GetGridTextValue(1, 8).Should().BeNullOrEmpty();


            this.results.GetGridValue(0, 9).Should().Be(17.34m);
            this.results.GetGridValue(1, 9).Should().BeNull();
        }
    }
}
