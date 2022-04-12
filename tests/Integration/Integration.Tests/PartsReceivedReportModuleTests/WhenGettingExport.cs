namespace Linn.Purchasing.Integration.Tests.PartsReceivedReportModuleTests
{
    using System;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingExport : ContextBase
    {
        private int supplierId;

        private string jobRef;

        private string fromDate;

        private string toDate;

        private string orderBy;

        private bool includeNegativeValues;

        [SetUp]
        public void SetUp()
        {
            this.supplierId = 1;
            this.jobRef = "A";
            this.fromDate = DateTime.UnixEpoch.ToString("o");
            this.toDate = new DateTime(1995, 3, 28).ToString("o");
            this.orderBy = "ORDER";
            this.includeNegativeValues = false;

            this.MockDomainService.GetReport(
                this.jobRef,
                this.supplierId,
                this.fromDate,
                this.toDate,
                this.orderBy,
                this.includeNegativeValues)
                .Returns(new ResultsModel());

            this.Response = this.Client.Get(
                $"/purchasing/reports/parts-received/export" 
                + $"?supplier={this.supplierId}&jobref={this.jobRef}&fromDate={this.fromDate}&toDate={this.toDate}"
                + $"&orderBy={this.orderBy}&includeNegativeValues={this.includeNegativeValues}",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldPassCorrectOptionsToDomainService()
        {
            this.MockDomainService.Received().GetReport(
                this.jobRef, 
                this.supplierId, 
                this.fromDate, 
                this.toDate, 
                this.orderBy, 
                this.includeNegativeValues);
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
