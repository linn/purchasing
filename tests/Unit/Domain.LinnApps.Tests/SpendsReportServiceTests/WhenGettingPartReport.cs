namespace Linn.Purchasing.Domain.LinnApps.Tests.SpendsReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPartReport : ContextBase
    {
        private readonly int supplierId = 77282;

        private readonly int orderNumber1 = 12;

        private readonly int orderNumber2 = 777;

        private readonly int orderNumber3 = 345;

        private ResultsModel results;

        [SetUp]
        public void SetUp()
        {
            var spends = new List<SupplierSpend>
                             {
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 120m,
                                         Supplier = new Supplier { SupplierId = this.supplierId, Name = "seller1" },
                                         LedgerPeriod = 1290,
                                         OrderNumber = this.orderNumber1,
                                         OrderLine = 1
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 130.87m,
                                         Supplier = new Supplier { SupplierId = this.supplierId, Name = "seller1" },
                                         LedgerPeriod = 1288,
                                         OrderNumber = this.orderNumber2,
                                         OrderLine = 1
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 500m,
                                         Supplier = new Supplier { SupplierId = this.supplierId, Name = "seller1" },
                                         LedgerPeriod = 1273,
                                         OrderNumber = this.orderNumber3,
                                         OrderLine = 1
                                     }
                             };

            var purchaseOrders = new List<PurchaseOrder>
                                     {
                                        new PurchaseOrder
                                            {
                                                SupplierId = this.supplierId,
                                                OrderNumber = this.orderNumber1,
                                                Details = new List<PurchaseOrderDetail>
                                                              {
                                                                  new PurchaseOrderDetail
                                                                      {
                                                                          Line = 1,
                                                                          OrderNumber = this.orderNumber1,
                                                                          PartNumber = "MCAS WAN"
                                                                      }
                                                              }
                                            },
                                        new PurchaseOrder
                                            {
                                                SupplierId = this.supplierId,
                                                OrderNumber = this.orderNumber2,
                                                Details = new List<PurchaseOrderDetail>
                                                              {
                                                                  new PurchaseOrderDetail
                                                                      {
                                                                          Line = 1,
                                                                          OrderNumber = this.orderNumber2,
                                                                          PartNumber = "MCAS 222"
                                                                      }
                                                              }
                                            },
                                        new PurchaseOrder
                                            {
                                                SupplierId = this.supplierId,
                                                OrderNumber = this.orderNumber3,
                                                Details = new List<PurchaseOrderDetail>
                                                              {
                                                                  new PurchaseOrderDetail
                                                                      {
                                                                          Line = 1,
                                                                          OrderNumber = this.orderNumber3,
                                                                          PartNumber = "RAW 33"
                                                                      }
                                                              }
                                            }
                                     };

            this.SpendsRepository.FilterBy(Arg.Any<Expression<Func<SupplierSpend, bool>>>())
                .Returns(spends.AsQueryable());

            this.PurchaseOrderRepository.FilterBy(Arg.Any<Expression<Func<PurchaseOrder, bool>>>())
                .Returns(purchaseOrders.AsQueryable());

            this.PurchaseLedgerPack.GetLedgerPeriod().Returns(1290);
            this.PurchaseLedgerPack.GetYearStartLedgerPeriod().Returns(1285);

            this.SupplierRepository.FindById(this.supplierId)
                .Returns(new Supplier { SupplierId = this.supplierId, Name = "The shop" });

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { Description = "A part of some sort" });

            this.results = this.Sut.GetSpendByPartReport(this.supplierId);
        }

        [Test]
        public void ShouldCallRepos()
        {
            this.PurchaseLedgerPack.Received().GetLedgerPeriod();
            this.PurchaseLedgerPack.Received().GetYearStartLedgerPeriod();
            this.SpendsRepository.Received().FilterBy(Arg.Any<Expression<Func<SupplierSpend, bool>>>());
            this.SupplierRepository.Received().FindById(this.supplierId);
            this.PartRepository.Received(3).FindBy(Arg.Any<Expression<Func<Part, bool>>>());
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.ReportTitle.DisplayValue.Should().Be(
                $"Spend by part report for Supplier: The shop ({this.supplierId}). For this financial year and last, excludes factors & VAT.");
            this.results.Rows.Count().Should().Be(3);
            this.results.Rows.First(x=>x.RowId == "MCAS WAN").Should().NotBeNull();
            this.results.GetGridTextValue(0, 0).Should().Be("RAW 33");
            this.results.GetGridTextValue(0, 1).Should().Be("A part of some sort");
            this.results.GetGridValue(0, 2).Should().Be(500);
            this.results.GetGridValue(0, 3).Should().Be(0m);
            this.results.GetGridValue(0, 4).Should().Be(0m);
            this.results.GetGridTextValue(1, 0).Should().Be("MCAS 222");

        }
    }
}
