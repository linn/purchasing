namespace Linn.Purchasing.Integration.Tests.PurchaseOrdersReportModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingUnacknowledgedOrders : ContextBase
    {
        private ReportReturnResource reportReturnResource;

        [SetUp]
        public void SetUp()
        {
            this.reportReturnResource = new ReportReturnResource
                                            {
                                                ReportResults = new List<ReportResultResource>
                                                                    {
                                                                        new ReportResultResource
                                                                            {
                                                                                title = new DisplayResource("Title")
                                                                            }
                                                                    }
                                            };
            this.FacadeService
                .GetUnacknowledgedOrdersReport(
                    Arg.Any<UnacknowledgedOrdersRequestResource>(),
                    Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<ReportReturnResource>(this.reportReturnResource));

            this.Response = this.Client.Get(
                $"/purchasing/reports/unacknowledged-orders?supplierId=123&supplierGroupId=456",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallFacadeService()
        {
            this.FacadeService.Received().GetUnacknowledgedOrdersReport(
                Arg.Is<UnacknowledgedOrdersRequestResource>(a => a.SupplierId == 123 && a.SupplierGroupId == 456),
                Arg.Any<IEnumerable<string>>());
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
            resource.ReportResults.First().title.displayString.Should().Be("Title");
        }
    }
}
