﻿namespace Linn.Purchasing.Integration.Tests.MaterialRequirementsModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingUsedOnReport : ContextBase
    {
        private ResultsModel results;

        private string partNumber;

        private string jobRef;

        [SetUp]
        public void SetUp()
        {
            this.partNumber = "RES 426";
            this.jobRef = "ABC";

            this.results = new ResultsModel { ReportTitle = new NameModel("Used On Report") };

            this.MockUsedOnReportDomainService.GetUsedOn(this.partNumber, this.jobRef).Returns(this.results);
            this.Response = this.Client.Get(
                $"/purchasing/material-requirements/used-on-report?partNumber={this.partNumber}&jobRef={this.jobRef}",
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
            this.MockUsedOnReportDomainService.Received().GetUsedOn(this.partNumber, this.jobRef);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(
                this.results.ReportTitle.DisplayValue);
        }
    }
}
