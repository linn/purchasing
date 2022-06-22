namespace Linn.Purchasing.Integration.Tests.MrOrderBookReportModuleTests
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

        private int supplierId;

        private string reportTitleA;

        private string reportTitleB;

        [SetUp]
        public void SetUp()
        {
            this.supplierId = 5000;
            this.reportTitleA = "PART A";
            this.reportTitleB = "PART B";

            this.results = new List<ResultsModel>
                               {
                                   new ResultsModel() { ReportTitle = new NameModel(this.reportTitleA) },
                                   new ResultsModel() { ReportTitle = new NameModel(this.reportTitleB) }
                               };
            this.MockDomainService.GetOrderBookReport(
                this.supplierId).Returns(this.results);

            this.Response = this.Client.Get(
                $"/purchasing/reports/mr-order-book?supplierId={this.supplierId}",
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
            this.MockDomainService.Received().GetOrderBookReport(
                this.supplierId);
        }

        [Test]
        public void ShouldReturnReports()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(this.reportTitleA);
            resource.ReportResults.Last().title.displayString.Should().Be(this.reportTitleB);
        }
    }
}
