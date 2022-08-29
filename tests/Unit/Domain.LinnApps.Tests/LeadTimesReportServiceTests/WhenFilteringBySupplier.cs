namespace Linn.Purchasing.Domain.LinnApps.Tests.LeadTimesReportServiceTests
{
    using FluentAssertions;

    using Linn.Common.Reporting.Models;

    using NUnit.Framework;

    public class WhenFilteringBySupplier : ContextBase
    {
        private ResultsModel result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.GetLeadTimesBySupplier(999);
        }

        [Test]
        public void ShouldOnlyReturnMatching()
        {
            this.result.ReportTitle.DisplayValue.Should().Be("Lead Times for Supplier : 999");
            this.result.RowCount().Should().Be(4);
            this.result.GetGridTextValue(0, 0).Should().Be("RES 311");
            this.result.GetGridTextValue(1, 0).Should().Be("RES 312");
            this.result.GetGridTextValue(2, 0).Should().Be("RES 313");
            this.result.GetGridTextValue(3, 0).Should().Be("RES 314");
            this.result.GetGridValue(0, 1).Should().Be(42);
            this.result.GetGridValue(1, 1).Should().Be(45);
            this.result.GetGridValue(2, 1).Should().Be(62);
            this.result.GetGridValue(3, 1).Should().Be(6);
        }
    }
}
