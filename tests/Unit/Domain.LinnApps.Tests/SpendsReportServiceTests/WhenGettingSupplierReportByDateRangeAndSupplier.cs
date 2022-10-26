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
                                         Supplier = new Supplier { SupplierId = 1234, Name = "seller1" },
                                         LedgerPeriod = 1295
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = 5678,
                                         Supplier = new Supplier { SupplierId = 5678, Name = "seller2" },
                                         LedgerPeriod = 1342
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         Supplier = new Supplier { SupplierId = this.supplierId, Name = "seller3" },
                                         LedgerPeriod = 1400
                                     }
                             };

            this.SpendsRepository.FilterBy(Arg.Any<Expression<Func<SupplierSpend, bool>>>())
                .Returns(spends.AsQueryable());
            var vendorManager = new VendorManager { Id = "A", UserNumber = 999, Employee = new Employee { FullName = "Aloo Gobi" } };
            this.LedgerPeriodPack.GetPeriodNumber(DateTime.Parse(this.fromDate)).Returns(7);
            this.LedgerPeriodPack.GetPeriodNumber(DateTime.Parse(this.toDate).Date.AddDays(1).AddTicks(-1)).Returns(8);
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
