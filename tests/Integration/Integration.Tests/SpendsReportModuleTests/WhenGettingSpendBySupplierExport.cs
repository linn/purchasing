namespace Linn.Purchasing.Integration.Tests.SpendsReportModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSpendBySupplierExport : ContextBase
    {
        private readonly string title =
            "Spend by supplier report for Vendor Manager: X - Doctor X (999). For this financial year and last, excludes factors & VAT.";

        [SetUp]
        public void SetUp()
        {
            this.DomainService.GetSpendBySupplierReport(string.Empty)
                .Returns(new ResultsModel { ReportTitle = new NameModel(this.title) });

            this.Response = this.Client.Get(
                "/purchasing/reports/spend-by-supplier/report?vm=",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService.Received().GetSpendBySupplierReport(string.Empty);
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
