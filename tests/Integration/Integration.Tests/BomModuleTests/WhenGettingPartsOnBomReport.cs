namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPartsOnBomReport : ContextBase
    {
        private ResultsModel results;

        private string bomName;

        private string reportTitle;

        [SetUp]
        public void SetUp()
        {
            this.bomName = "sk hub ";

            this.reportTitle = "Parts on SK HUB Bom";

            this.results = new ResultsModel() { ReportTitle = new NameModel(this.reportTitle) };

            this.MockBomReportsDomainService.GetPartsOnBomReport(
                this.bomName.Trim().ToUpper()).Returns(this.results);

            this.Response = this.Client.Get(
                $"/purchasing/boms/reports/list?bomName={this.bomName}",
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
            this.MockBomReportsDomainService.Received().GetPartsOnBomReport(
                this.bomName.ToUpper().Trim());
        }

        [Test]
        public void ShouldReturnReports()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(this.reportTitle);
        }
    }
}
