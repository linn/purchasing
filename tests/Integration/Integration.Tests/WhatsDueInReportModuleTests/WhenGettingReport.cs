namespace Linn.Purchasing.Integration.Tests.WhatsDueInReportModuleTests
{
    using System;
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

        private DateTime fromDate;

        private DateTime toDate;

        private string orderBy;

        private string vendorManager;

        private int supplier;

        [SetUp]
        public void SetUp()
        {
            this.fromDate = DateTime.Today;
            this.toDate = DateTime.Today;
            this.orderBy = "EXPECTED DATE";
            this.vendorManager = "A";
            this.supplier = 123;

            this.resultsModel = new ResultsModel { ReportTitle = new NameModel("Whats Due In Report") };
            this.MockDomainService.GetReport(
                this.fromDate.Date,
                this.toDate.Date.AddDays(1),
                this.orderBy,
                this.vendorManager,
                this.supplier).Returns(this.resultsModel);

            this.Response = this.Client.Get(
                $"/purchasing/reports/whats-due-in?fromDate=" 
                + $"{this.fromDate}&toDate={this.toDate}&orderBy={this.orderBy}"
                + $"&vendorManager={this.vendorManager}" 
                + $"&supplier={this.supplier}",
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
            this.MockDomainService.Received().GetReport(
                this.fromDate.Date,
                this.toDate.Date.AddDays(1),
                this.orderBy,
                this.vendorManager,
                this.supplier);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(this.resultsModel.ReportTitle.DisplayValue);
        }
    }
}

