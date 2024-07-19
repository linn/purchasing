namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBomPrintReport : ContextBase
    {
        private IEnumerable<ResultsModel> results;

        private string bomName;

        [SetUp]
        public void SetUp()
        {
            this.results = new List<ResultsModel> { new ResultsModel() };

            this.bomName = "SK HUB";

            this.MockBomReportsDomainService.GetBomPrintReport(this.bomName).Returns(this.results);

            this.Response = this.Client.Get(
                $"/purchasing/boms/reports/bom-print?bomName={this.bomName}",
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
            this.MockBomReportsDomainService.Received().GetBomPrintReport(
                this.bomName.ToUpper());
        }
    }
}
