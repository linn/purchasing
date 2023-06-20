namespace Linn.Purchasing.Integration.Tests.BomReportsModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBoardComponentSummaryReport : ContextBase
    {
        private string boardCode;

        private string revisionCode;

        [SetUp]
        public void SetUp()
        {
            this.boardCode = "1";
            this.revisionCode = "L1R1";

            var reportResult = new ResultsModel
                                   {
                                       ReportTitle = new NameModel("title")
                                   };

            this.DomainService
                .GetBoardComponentSummaryReport(this.boardCode, this.revisionCode)
                .Returns(reportResult);

            this.Response = this.Client.Get(
                $"/purchasing/boms/reports/board-component-summary/report?boardCode={this.boardCode}&revisionCode={this.revisionCode}",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService.Received().GetBoardComponentSummaryReport(
                this.boardCode,
                this.revisionCode);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be("title");
        }
    }
}
