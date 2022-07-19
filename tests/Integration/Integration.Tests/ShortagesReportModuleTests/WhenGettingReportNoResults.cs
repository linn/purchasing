namespace Linn.Purchasing.Integration.Tests.ShortagesReportModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReportNoResults : ContextBase
    {
        private IEnumerable<ResultsModel> results;

        private int purchaseLevel;

        private string vendorManager;

        private string reportTitle;

        [SetUp]
        public void SetUp()
        {
            this.purchaseLevel = 3;
            this.vendorManager = "Z";
            this.reportTitle = "No Results";

            this.results = new List<ResultsModel> { new ResultsModel() { ReportTitle = new NameModel(this.reportTitle) } };
            this.MockDomainService.GetShortagesReport(
                this.purchaseLevel,
                this.vendorManager).Returns(this.results);

            this.Response = this.Client.Get(
                $"/purchasing/reports/shortages/report?" 
                + $"purchaseLevel={this.purchaseLevel}&vendorManager={this.vendorManager}",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldPassCorrectOptionsToDomainService()
        {
            this.MockDomainService.Received().GetShortagesReport(
                this.purchaseLevel,
                this.vendorManager);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(this.reportTitle);
        }
    }
}
