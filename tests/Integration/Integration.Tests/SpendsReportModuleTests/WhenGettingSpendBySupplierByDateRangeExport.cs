namespace Linn.Purchasing.Integration.Tests.SpendsReportModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSpendBySupplierByDateRangeExport : ContextBase
    {
        private readonly string title =
            "Spend by supplier report for Vendor Manager: A - Aloo Gobi (999) between ledger period 0 to 0.";
      
        private SpendBySupplierByDateRangeReportRequestResource requestResource;

        [SetUp]
        public void SetUp()
        {
            this.requestResource = new SpendBySupplierByDateRangeReportRequestResource
            {
                FromDate = string.Empty,
                ToDate = string.Empty,
                VendorManager = string.Empty
            };

            this.DomainService.GetSpendBySupplierByDateRangeReport(this.requestResource.FromDate, this.requestResource.ToDate, this.requestResource.VendorManager)
                .Returns(new ResultsModel { ReportTitle = new NameModel(this.title) });

            this.Response = this.Client.Get(
                $"/purchasing/reports/spend-by-supplier-by-date-range/export?fromDate={this.requestResource.FromDate}&toDate={this.requestResource.ToDate}&vm={this.requestResource.VendorManager}",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService.Received().GetSpendBySupplierByDateRangeReport(string.Empty, string.Empty, string.Empty);
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
