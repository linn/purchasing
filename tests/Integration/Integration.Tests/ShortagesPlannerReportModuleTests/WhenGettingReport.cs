namespace Linn.Purchasing.Integration.Tests.ShortagesPlannerReportModuleTests
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

    public class WhenGettingReport : ContextBase
    {
        private IEnumerable<ResultsModel> results;

        private int planner;

        private string reportTitle;

        [SetUp]
        public void SetUp()
        {
            this.planner = 5003;
            this.reportTitle = "Test Shortages Report";

            this.results = new List<ResultsModel> { new ResultsModel() { ReportTitle = new NameModel(this.reportTitle) } };
            this.MockDomainService.GetShortagesPlannerReport(
                this.planner).Returns(this.results);
            this.Response = this.Client.Get(
                $"/purchasing/reports/shortages-planner/report?" 
                + $"planner={this.planner}",
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
            this.MockDomainService.Received().GetShortagesPlannerReport(this.planner);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(this.reportTitle);
        }
    }
}
