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

    public class WhenGettingBomDifferencesReport : ContextBase
    {
        private ResultsModel results;

        private string bom1;

        private string bom2;

        private string reportTitle;

        [SetUp]
        public void SetUp()
        {
            this.bom1 = "sk hub ";

            this.bom2 = "majik dsm";

            this.reportTitle = "BOM DIFF REP";

            this.results = new ResultsModel { ReportTitle = new NameModel(this.reportTitle) };

            this.DomainService.GetBomDifferencesReport(
                this.bom1.Trim().ToUpper(), this.bom2.Trim().ToUpper()).Returns(this.results);

            this.Response = this.Client.Get(
                $"/purchasing/boms/reports/diff?bom1={this.bom1}&bom2={this.bom2}",
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
            this.DomainService.Received()
                .GetBomDifferencesReport(this.bom1.Trim().ToUpper(), this.bom2.Trim().ToUpper());
        }

        [Test]
        public void ShouldReturnReports()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(this.reportTitle);
        }
    }
}
