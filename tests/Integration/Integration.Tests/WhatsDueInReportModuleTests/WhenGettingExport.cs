namespace Linn.Purchasing.Integration.Tests.WhatsDueInReportModuleTests
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

        private string vendorManager;

        private DateTime fromDate;

        private DateTime toDate;

        private string orderBy;

        [SetUp]
        public void SetUp()
        {
            this.supplierId = 1;
            this.vendorManager = "A";
            this.fromDate = DateTime.UnixEpoch;
            this.toDate = new DateTime(1995, 3, 28);
            this.orderBy = "ORDER";

            this.MockDomainService.GetReport(
                    this.fromDate.Date,
                    this.toDate.Date.AddDays(1).AddTicks(-1),
                    this.orderBy,
                    this.vendorManager,
                    this.supplierId)
                .Returns(new ResultsModel());

            this.Response = this.Client.Get(
                $"/purchasing/reports/whats-due-in/export"
                + $"?supplier={this.supplierId}&vendorManager={this.vendorManager}&fromDate={this.fromDate:o}&toDate={this.toDate:o}"
                + $"&orderBy={this.orderBy}",
                with => { with.Accept("text/csv"); })?.Result;
        }

        [Test]
        public void ShouldPassCorrectOptionsToDomainService()
        {
            this.MockDomainService.Received().GetReport(
                this.fromDate.Date,
                this.toDate.Date.AddDays(1).AddTicks(-1),
                this.orderBy,
                this.vendorManager,
                this.supplierId);
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
