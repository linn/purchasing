namespace Linn.Purchasing.Domain.LinnApps.Tests.OrdersBySupplerReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private readonly int supplierId = 77282;
        private readonly int orderNumber = 9876;

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
                                                                           NetTotal = 2m,
                                                                           OrderNumber = this.orderNumber,
                                                                           OurQty = 3,
                                                                           PartNumber = "WHO N0z",
                                                                           PurchaseDelivery =
                                                                               new PurchaseOrderDelivery
                                                                                   {
                                                                                       QtyNetReceived = 11,
                                                                                       DeliverySeq = 12,
                                                                                       OurDeliveryQty = 13,
                                                                                       NetTotal = 14,
                                                                                       DateRequested =
                                                                                           new DateTime(2021, 11, 1),
                                                                                       DateAdvised = new DateTime(
                                                                                           2021,
                                                                                           12,
                                                                                           1)
                                                                                   },
                                                                           SuppliersDesignation =
                                                                               "a sorta description that's quite long total £15millION"
                                                                       }
                                                               },
                                                 DocumentType = "Suhn"
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

            //this.PurchaseOrdersPack.OrderIsCompleteSql(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

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
            this.results.Rows.Count().Should().Be(1);
            var row = this.results.Rows.First();
            row.RowId.Should().Be($"{this.orderNumber}/1");
        }
    }
}
