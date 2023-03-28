namespace Linn.Purchasing.Integration.Tests.PurchaseOrdersReportModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingUnacknowledgedOrdersExport : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.FacadeService.GetUnacknowledgedOrdersReport(Arg.Any<UnacknowledgedOrdersRequestResource>()).Returns(
                new SuccessResult<ReportReturnResource>(
                    new ReportReturnResource
                        {
                            ReportResults = new List<ReportResultResource>
                                                {
                                                    new ReportResultResource
                                                        {
                                                            headers = new HeaderResource { columnHeaders = new List<string>() },
                                                            title = new DisplayResource("Title"),
                                                            results  = new List<ResultDetailsResource>()
                                                        }
                                                }
                        }));

            this.Response = this.Client.Get(
                $"/purchasing/reports/unacknowledged-orders?supplierId=123&supplierGroupId=456",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldCallFacadeService()
        {
            this.FacadeService.Received().GetUnacknowledgedOrdersReport(
                Arg.Is<UnacknowledgedOrdersRequestResource>(a => a.SupplierId == 123 && a.SupplierGroupId == 456));
        }

        [Test]
        public void ShouldReturnJsonContentType()
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
