namespace Linn.Purchasing.Integration.Tests.LeadTimesReportModuleTests
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

        private int supplier;

        [SetUp]
        public void SetUp()
        {
            this.supplier = 123;
            this.resultsModel = new ResultsModel { ReportTitle = new NameModel($"Lead Times for Supplier {this.supplier}") };
            this.MockDomainService.GetLeadTimesBySupplier(
                this.supplier).Returns(this.resultsModel);

            this.Response = this.Client.Get(
                $"/purchasing/reports/leadtimes/report?="
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
            this.MockDomainService.Received().GetLeadTimesBySupplier(this.supplier);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(this.resultsModel.ReportTitle.DisplayValue);
        }
    }
}
