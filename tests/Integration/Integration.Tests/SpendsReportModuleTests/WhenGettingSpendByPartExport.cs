namespace Linn.Purchasing.Integration.Tests.SpendsReportModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSpendByPartExport : ContextBase
    {
        private readonly string title =
            "Spend by part report for Suppler: Doctor X (999). For this financial year and last, excludes factors & VAT.";

        [SetUp]
        public void SetUp()
        {
            this.DomainService.GetSpendByPartReport(999)
                .Returns(new ResultsModel { ReportTitle = new NameModel(this.title) });

            this.Response = this.Client.Get(
                "/purchasing/reports/spend-by-part/report?id=999",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService.Received().GetSpendByPartReport(999);
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
