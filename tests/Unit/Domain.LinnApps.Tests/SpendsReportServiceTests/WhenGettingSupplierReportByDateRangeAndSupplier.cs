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

    public class WhenGettingSupplierReportByDateRangeAndSupplier : ContextBase
    {
        private ResultsModel results;

        private string fromDate = "2007-07-18T23:00:00.000Z";

        private string toDate = "2022-08-19T08:21:13.592Z";

        private int supplierId;

        [SetUp]
        public void SetUp()
        {
            this.supplierId = 123;
            var spends = new List<SupplierSpend>
                             {
                                 new SupplierSpend
                                     {
                                         SupplierId = 1234,
                                         LedgerPeriod = 1295,
                                         SupplierName = "seller1",
                                         VendorManager = "A"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = 5678,
                                         LedgerPeriod = 1342,
                                         SupplierName = "seller2",
                                         VendorManager = "A"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         LedgerPeriod = 1400,
                                         SupplierName = "seller3",
                                         VendorManager = "A"
                                     }
                             };

            this.SpendsRepository.FilterBy(Arg.Any<Expression<Func<SupplierSpend, bool>>>())
                .Returns(spends.AsQueryable());
            var vendorManager = new VendorManager { Id = "A", UserNumber = 999, Employee = new Employee { FullName = "Aloo Gobi" } };
            this.LedgerPeriodPack.GetPeriodNumber(Arg.Any<DateTime>()).Returns(7, 8);
            this.LedgerPeriodRepository.FindById(7).Returns(new LedgerPeriod { MonthName = "Jul2007" });
            this.LedgerPeriodRepository.FindById(8).Returns(new LedgerPeriod { MonthName = "Aug2022" });

            this.VendorManagerRepository.FindById(Arg.Any<string>()).Returns(vendorManager);

            this.results = this.Sut.GetSpendBySupplierByDateRangeReport(
                this.fromDate,
                this.toDate,
                "A",
                this.supplierId);
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.Rows.Count().Should().Be(1);
            this.results.GetGridTextValue(0, 0).Should().Be("seller3");
        }
    }
}
