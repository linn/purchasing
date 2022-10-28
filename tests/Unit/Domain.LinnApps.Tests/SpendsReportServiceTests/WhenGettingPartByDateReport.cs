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

    public class WhenGettingPartByDateReport : ContextBase
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
                                         PartNumber = "MCAS 111",
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
                                         PartNumber = "MCAS 222",
                                         PartDescription = "Desc"
                                     },
                                 new SupplierSpend
                                     {
                                         SupplierId = this.supplierId,
                                         BaseTotal = 500m,
                                         LedgerPeriod = 1273,
                                         OrderNumber = this.orderNumber3,
                                         OrderLine = 1,
                                         SupplierName = "seller1",
                                         PartNumber = "RAW 033",
                                         PartDescription  = "A part of some sort"
                                     }
                             };

            this.SpendsRepository.FilterBy(Arg.Any<Expression<Func<SupplierSpend, bool>>>())
                .Returns(spends.AsQueryable());
            this.SupplierRepository.FindById(this.supplierId)
                .Returns(new Supplier { SupplierId = this.supplierId, Name = "The shop" });

            this.LedgerPeriodPack.GetPeriodNumber(Arg.Any<DateTime>()).Returns(12, 24);
            this.results = this.Sut.GetSpendByPartByDateReport(this.supplierId, "2023-10-27T15:54:55Z", "2024-10-27T15:54:55Z");
        }

        [Test]
        public void ShouldCallRepos()
        {
            this.SpendsRepository.Received().FilterBy(Arg.Any<Expression<Func<SupplierSpend, bool>>>());
            this.SupplierRepository.Received().FindById(this.supplierId);
        }

        [Test]
        public void ShouldReturnData()
        {
            this.results.ReportTitle.DisplayValue.Should().Be("Spend by part report for Supplier: The shop (77282) in GBP between 27-Oct-2023 and 27-Oct-2024");
            this.results.Rows.Count().Should().Be(3);
            this.results.GetGridTextValue(0, 0).Should().Be("MCAS 111");
            this.results.GetGridTextValue(0, 1).Should().Be("Another part of some sort");
            this.results.GetGridValue(0, 2).Should().Be(120m);
            this.results.GetGridTextValue(1, 0).Should().Be("MCAS 222");
            this.results.GetGridTextValue(1, 1).Should().Be("Desc");
            this.results.GetGridValue(1, 2).Should().Be(130.87m);
            this.results.GetGridTextValue(2, 0).Should().Be("RAW 033");
            this.results.GetGridTextValue(2, 1).Should().Be("A part of some sort");
            this.results.GetGridValue(2, 2).Should().Be(500m);
        }
    }
}
