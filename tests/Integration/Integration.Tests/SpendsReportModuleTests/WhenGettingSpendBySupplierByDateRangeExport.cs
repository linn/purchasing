namespace Linn.Purchasing.Integration.Tests.SpendsReportModuleTests
{
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

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
                FromDate = 1.December(2025).ToString("o"),
                ToDate = 1.December(2026).ToString("o"),
                VendorManager = "A",
                SupplierId = 123
            };

            this.DomainService.GetSpendBySupplierByDateRangeReport(
                    this.requestResource.FromDate,
                    this.requestResource.ToDate,
                    this.requestResource.VendorManager,
                    this.requestResource.SupplierId)
                .Returns(new ResultsModel { ReportTitle = new NameModel(this.title) });

            this.Response = this.Client.Get(
                $"/purchasing/reports/spend-by-supplier-by-date-range/export?fromDate={this.requestResource.FromDate}&toDate={this.requestResource.ToDate}&vm={this.requestResource.VendorManager}&supplierId={this.requestResource.SupplierId}",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService.Received().GetSpendBySupplierByDateRangeReport(
                this.requestResource.FromDate,
                this.requestResource.ToDate,
                this.requestResource.VendorManager,
                this.requestResource.SupplierId);
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
