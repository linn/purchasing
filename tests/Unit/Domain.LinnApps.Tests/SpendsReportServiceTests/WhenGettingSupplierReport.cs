namespace Linn.Purchasing.Domain.LinnApps.Tests.SpendsReportServiceTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSupplierReport : ContextBase
    {
        private readonly int supplierId = 77282;

        private ResultsModel results;

        [SetUp]
        public void SetUp()
        {
            var yearStartLedgerPeriod = 1492;
            var ledgerPeriodInLastYear = 1489;
            var ledgerPeriodInYearBeforeLast = 1479;
            var currentLedgerPeriod = 1500;
            var aLongTimeAgo = 1400;

            var spends = new List<SupplierSpend>
                             {
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 120m,
                                         LedgerPeriod = ledgerPeriodInLastYear,
                                         SupplierName = "seller1",
                                         VendorManager = "X"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 120m,
                                         LedgerPeriod = ledgerPeriodInLastYear,
                                         SupplierName = "seller1",
                                         VendorManager = "X"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 120m,
                                         LedgerPeriod = ledgerPeriodInYearBeforeLast,
                                         SupplierName = "seller1",
                                         VendorManager = "X"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 130.87m,
                                         LedgerPeriod = ledgerPeriodInYearBeforeLast,
                                         SupplierName = "seller1",
                                         VendorManager = "X"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 500m,
                                         LedgerPeriod = currentLedgerPeriod,
                                         SupplierName = "seller1",
                                         VendorManager = "X"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 500000m,
                                         LedgerPeriod = aLongTimeAgo, // should be ignored
                                         SupplierName = "seller1",
                                         VendorManager = "X"
                                     }
                             };

            this.SpendsRepository.FindAll()
                .Returns(spends.AsQueryable());
            var vendorManager = new VendorManager { Id = "X", UserNumber = 999, Employee = new Employee { FullName = "Doctor X" } };

            this.VendorManagerRepository.FindById(Arg.Any<string>()).Returns(vendorManager);

            this.PurchaseLedgerPack.GetLedgerPeriod().Returns(currentLedgerPeriod);
            this.PurchaseLedgerPack.GetYearStartLedgerPeriod().Returns(yearStartLedgerPeriod);

            this.results = this.Sut.GetSpendBySupplierReport("X");
        }

        [Test]
        public void ShouldCallRepos()
        {
            this.PurchaseLedgerPack.Received().GetLedgerPeriod();
            this.PurchaseLedgerPack.Received().GetYearStartLedgerPeriod();
            this.SpendsRepository.Received().FindAll();
            this.VendorManagerRepository.Received().FindById(Arg.Any<string>());
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.ReportTitle.DisplayValue.Should().Be(
                $"Spend by supplier report for Vendor Manager: X - Doctor X - GBP (excluding factors & VAT.)");
            this.results.Rows.Count().Should().Be(1);
            var row = this.results.Rows.First();
            row.RowId.Should().Be(this.supplierId.ToString());
            this.results.GetGridTextValue(0, 0).Should().Be("seller1");
            this.results.GetGridValue(0, 1).Should().Be(250.87m); // in 12 months before last FY, i.e. the financial year before last
            this.results.GetGridValue(0, 2).Should().Be(240m);    // in last FY
            this.results.GetGridValue(0, 3).Should().Be(500m);     // in current period, i.e. this month
        }
    }
}
