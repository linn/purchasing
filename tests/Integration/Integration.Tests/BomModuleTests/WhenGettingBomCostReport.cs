namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBomCostReport : ContextBase
    {
        private IEnumerable<BomCostReport> results;

        private string bomName;

        [SetUp]
        public void SetUp()
        {
            this.results = new List<BomCostReport> { new BomCostReport { SubAssembly = "SK HUB", Breakdown = new ResultsModel() } };

            this.bomName = "SK HUB";

            this.MockBomReportsDomainService.GetBomCostReport(this.bomName, true, 999, 15).Returns(this.results);

            this.Response = this.Client.Get(
                $"/purchasing/boms/reports/cost?bomName={this.bomName}" 
                + $"&splitBySubAssembly=True&levels=999&labourHourlyRate=15",
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
            this.MockBomReportsDomainService.Received().GetBomCostReport(
                this.bomName.ToUpper().Trim(), true, 999, 15);
        }
    }
}
