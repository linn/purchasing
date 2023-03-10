namespace Linn.Purchasing.Integration.Tests.ChangeStatusReportModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingProposedChanges : ContextBase
    {
        private int months;

        [SetUp]
        public void SetUp()
        {
            this.months = 3;

            var reportResult = new ResultsModel
                                   {
                                       ReportTitle = new NameModel("title")
                                   };

            this.DomainService
                .GetProposedChangesReport(this.months)
                .Returns(reportResult);

            this.Response = this.Client.Get(
                $"/purchasing/reports/proposed-changes/report?months={this.months}",
                with => { with.Accept("application/json"); }).Result;
        }
        
        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService.Received().GetProposedChangesReport(
                this.months);
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
