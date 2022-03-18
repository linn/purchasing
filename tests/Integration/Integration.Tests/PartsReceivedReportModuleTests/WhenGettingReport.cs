namespace Linn.Purchasing.Integration.Tests.PartsReceivedReportModuleTests
{
    using System;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private ResultsModel resultsModel;

        private PartsReceivedReportRequestResource optionsResource;

        [SetUp]
        public void SetUp()
        {
            this.optionsResource = new PartsReceivedReportRequestResource
                                      {
                                          Supplier = 123,
                                          FromDate = DateTime.UnixEpoch.ToString("o"),
                                          ToDate = DateTime.UnixEpoch.AddDays(1).ToString("O"),
                                          IncludeNegativeValues = true,
                                          Jobref = "AA"
                                      };
            this.resultsModel = new ResultsModel { ReportTitle = new NameModel("Parts Received Report") };
            this.MockDomainService.GetReport(
                this.optionsResource.Jobref,
                this.optionsResource.Supplier,
                this.optionsResource.FromDate,
                this.optionsResource.ToDate,
                this.optionsResource.IncludeNegativeValues).Returns(this.resultsModel);

            this.Response = this.Client.Get(
                $"/purchasing/reports/parts-received?supplier=" 
                + $"{this.optionsResource.Supplier}&fromDate={this.optionsResource.FromDate}&toDate={this.optionsResource.ToDate}"
                + $"&includeNegativeValues={this.optionsResource.IncludeNegativeValues}" 
                + $"&jobref={this.optionsResource.Jobref}",
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
                this.optionsResource.Jobref,
                this.optionsResource.Supplier,
                this.optionsResource.FromDate,
                this.optionsResource.ToDate,
                this.optionsResource.IncludeNegativeValues);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(this.resultsModel.ReportTitle.DisplayValue);
        }
    }
}
