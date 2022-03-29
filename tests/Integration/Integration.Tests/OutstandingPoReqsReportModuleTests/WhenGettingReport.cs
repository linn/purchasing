namespace Linn.Purchasing.Integration.Tests.OutstandingPoReqsReportModuleTests
{
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
        private ResultsModel resultsModel;

        private string state;

        [SetUp]
        public void SetUp()
        {
            this.state = "STATE";

            this.resultsModel = new ResultsModel { ReportTitle = new NameModel("Outstanding PO Reqs Report") };
            this.MockDomainService.GetReport(this.state).Returns(this.resultsModel);

            this.Response = this.Client.Get(
                $"/purchasing/reports/outstanding-po-reqs/report?state={this.state}",
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
            this.MockDomainService.Received().GetReport(this.state);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(this.resultsModel.ReportTitle.DisplayValue);
        }
    }
}

