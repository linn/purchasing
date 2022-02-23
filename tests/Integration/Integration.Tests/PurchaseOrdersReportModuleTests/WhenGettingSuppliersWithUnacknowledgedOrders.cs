namespace Linn.Purchasing.Integration.Tests.PurchaseOrdersReportModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSuppliersWithUnacknowledgedOrders : ContextBase
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
                .GetSuppliersWithUnacknowledgedOrdersReport(
                    Arg.Any<SuppliersWithUnacknowledgedOrdersRequestResource>(),
                    Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<ReportReturnResource>(this.reportReturnResource));

            this.Response = this.Client.Get(
                $"/purchasing/reports/suppliers-with-unacknowledged-orders?planner=123&vendorManager=A&useSupplierGroup=true",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallFacadeService()
        {
            this.FacadeService.Received().GetSuppliersWithUnacknowledgedOrdersReport(
                Arg.Is<SuppliersWithUnacknowledgedOrdersRequestResource>(
                    a => a.Planner == 123 && a.VendorManager == "A" && a.UseSupplierGroup == true),
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
