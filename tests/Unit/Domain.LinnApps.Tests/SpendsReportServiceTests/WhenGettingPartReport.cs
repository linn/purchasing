namespace Linn.Purchasing.Domain.LinnApps.Tests.SpendsReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
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
                                         LedgerPeriod = 1290,
                                         OrderNumber = this.orderNumber1,
                                         OrderLine = 1,
                                         SupplierName = "seller1",
                                         PartNumber = "MCAS WAN",
                                         PartDescription = "Another part of some sort"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 130.87m,
                                         LedgerPeriod = 1288,
                                         OrderNumber = this.orderNumber2,
                                         OrderLine = 1,
                                         SupplierName = "seller1",
                                         PartNumber = "MCAS 222"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 500m,
                                         LedgerPeriod = 1273,
                                         OrderNumber = this.orderNumber3,
                                         OrderLine = 1,
                                         SupplierName = "seller1",
                                         PartNumber = "RAW 33",
                                         PartDescription  = "A part of some sort"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 40.40m,
                                         LedgerPeriod = 1263,
                                         OrderNumber = this.orderNumber3,
                                         OrderLine = 1,
                                         SupplierName = "seller1",
                                         PartNumber = "RAW 33",
                                         PartDescription  = "A part of some sort"
                                     }
                             };

            this.SpendsRepository.FilterBy(Arg.Any<Expression<Func<SupplierSpend, bool>>>())
                .Returns(spends.AsQueryable());

            this.PurchaseLedgerPack.GetLedgerPeriod().Returns(1290);
            this.PurchaseLedgerPack.GetYearStartLedgerPeriod().Returns(1285);

            this.SupplierRepository.FindById(this.supplierId)
                .Returns(new Supplier { SupplierId = this.supplierId, Name = "The shop" });

            this.results = this.Sut.GetSpendByPartReport(this.supplierId);
        }

        [Test]
        public void ShouldCallRepos()
        {
            this.PurchaseLedgerPack.Received().GetLedgerPeriod();
            this.PurchaseLedgerPack.Received().GetYearStartLedgerPeriod();
            this.SpendsRepository.Received().FilterBy(Arg.Any<Expression<Func<SupplierSpend, bool>>>());
            this.SupplierRepository.Received().FindById(this.supplierId);
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.ReportTitle.DisplayValue.Should().Be(
                $"Spend by part report for Supplier: The shop ({this.supplierId}). In GBP, for this financial year and last, excludes factors & VAT.");
            this.results.Rows.Count().Should().Be(3);
            this.results.Rows.First(x => x.RowId == "MCAS WAN").Should().NotBeNull();
            this.results.GetGridTextValue(0, 0).Should().Be("RAW 33");
            this.results.GetGridTextValue(0, 1).Should().Be("A part of some sort");
            this.results.GetGridValue(0, 2).Should().Be(40.40m);
            this.results.GetGridValue(0, 3).Should().Be(500);
            this.results.GetGridValue(0, 4).Should().Be(0m);
            this.results.GetGridValue(0, 5).Should().Be(0m);
            this.results.GetGridTextValue(1, 0).Should().Be("MCAS 222");
        }
    }
}
