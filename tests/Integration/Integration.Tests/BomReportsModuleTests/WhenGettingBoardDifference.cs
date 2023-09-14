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

    public class WhenGettingBoardDifference : ContextBase
    {
        private string boardCode1;

        private string revisionCode1;

        private string boardCode2;

        private string revisionCode2;

        private bool liveOnly;

        [SetUp]
        public void SetUp()
        {
            this.boardCode1 = "1";
            this.revisionCode1 = "L1R1";
            this.boardCode2 = "1";
            this.revisionCode2 = "L1R2";
            this.liveOnly = true;

            var reportResult = new ResultsModel
                                   {
                                       ReportTitle = new NameModel("title")
                                   };

            this.DomainService
                .GetBoardDifferenceReport(
                    this.boardCode1,
                    this.revisionCode1,
                    this.boardCode2,
                    this.revisionCode2,
                    this.liveOnly)
                .Returns(reportResult);

            this.Response = this.Client.Get(
                $"/purchasing/reports/board-difference/report?boardCode1={this.boardCode1}&revisionCode1={this.revisionCode1}&boardCode2={this.boardCode2}&revisionCode2={this.revisionCode2}&liveOnly={this.liveOnly}",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService.Received().GetBoardDifferenceReport(
                this.boardCode1,
                this.revisionCode1,
                this.boardCode2,
                this.revisionCode2,
                this.liveOnly);
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
