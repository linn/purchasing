namespace Linn.Purchasing.Integration.Tests.SpendsReportModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSpendBySupplierReport : ContextBase
    {
        private readonly string title =
            "Spend by supplier report for Vendor Manager: X - Doctor X (999). For this financial year and last, excludes factors & VAT.";

        [SetUp]
        public void SetUp()
        {
            this.DomainService.GetSpendBySupplierReport("X")
                .Returns(new ResultsModel { ReportTitle = new NameModel(this.title) });

            this.Response = this.Client.Get(
                "/purchasing/reports/spend-by-supplier/report?vm=X",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService.Received().GetSpendBySupplierReport("X");
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<ReportReturnResource>();
            resource.ReportResults.First().title.displayString.Should().Be(this.title);
        }
    }
}
