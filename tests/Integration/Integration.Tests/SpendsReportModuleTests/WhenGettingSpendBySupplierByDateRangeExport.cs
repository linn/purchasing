namespace Linn.Purchasing.Integration.Tests.SpendsReportModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSpendBySupplierByDateRangeExport : ContextBase
    {
        private readonly string title =
            "Spend by supplier report for Vendor Manager: A - Aloo Gobi (999) between ledger period 0 to 0.";

        [SetUp]
        public void SetUp()
        {
            this.DomainService.GetSpendBySupplierByDateRangeReport(string.Empty, string.Empty, string.Empty)
                .Returns(new ResultsModel { ReportTitle = new NameModel(this.title) });

            this.Response = this.Client.Get(
                "/purchasing/reports/spend-by-supplier-by-date-range/export?vm=&fromDate=&toDate=",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService.Received().GetSpendBySupplierByDateRangeReport(string.Empty, string.Empty, string.Empty);
        }

        [Test]
        public void ShouldReturnCsvContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("text/csv");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
