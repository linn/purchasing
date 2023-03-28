namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPartsOnBomExport : ContextBase
    {
        private ResultsModel results;

        private string bomName;

        private string reportTitle;

        [SetUp]
        public void SetUp()
        {
            this.bomName = "sk hub ";

            this.reportTitle = "Parts on SK HUB Bom";

            this.results = new ResultsModel { ReportTitle = new NameModel(this.reportTitle) };

            this.MockBomReportsDomainService.GetPartsOnBomReport(
                this.bomName.Trim().ToUpper()).Returns(this.results);

            this.Response = this.Client.Get(
                $"/purchasing/boms/reports/list?bomName={this.bomName}",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldPassCorrectOptionsToDomainService()
        {
            this.MockBomReportsDomainService.Received().GetPartsOnBomReport(
                this.bomName.ToUpper().Trim());
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

