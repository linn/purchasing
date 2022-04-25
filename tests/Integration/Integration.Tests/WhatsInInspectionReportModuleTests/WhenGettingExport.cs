namespace Linn.Purchasing.Integration.Tests.WhatsInInspectionReportModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingExport : ContextBase
    {

        private bool includePartsWithNoOrderNumber;

        private bool includeFailedStock;

        private bool includeFinishedGoods;

        [SetUp]
        public void SetUp()
        {
            this.includePartsWithNoOrderNumber = true;
            this.includeFailedStock = true;
            this.includeFinishedGoods = false;
            
            this.MockDomainService.GetTopLevelReport(
                this.includePartsWithNoOrderNumber,
                this.includeFailedStock,
                this.includeFinishedGoods).Returns(new ResultsModel());

            this.Response = this.Client.Get(
                $"/purchasing/reports/whats-in-inspection/export?includePartsWithNoOrderNumber="
                + $"{this.includePartsWithNoOrderNumber}"
                + $"&includeFailedStock={this.includeFailedStock}"
                + $"&includeFinishedGoods={this.includeFinishedGoods}",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldPassCorrectOptionsToDomainService()
        {
            this.MockDomainService.Received().GetTopLevelReport(
                this.includePartsWithNoOrderNumber,
                this.includeFailedStock,
                this.includeFinishedGoods);
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
