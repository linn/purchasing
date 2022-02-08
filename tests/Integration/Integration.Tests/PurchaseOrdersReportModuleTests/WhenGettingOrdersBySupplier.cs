namespace Linn.Purchasing.Integration.Tests.PurchaseOrdersReportModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingOrdersBySupplier : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var reportReturnResource = new ReportReturnResource();
            var reportResult = new ReportResultResource
                                   {
                                       displaySequence = 1,
                                       title = new DisplayResource("potat"),
                                       results = new List<ResultDetailsResource>
                                                     {
                                                         new ResultDetailsResource
                                                             {
                                                                 rowTitle = new DisplayResource("rowtitle"),
                                                                 rowType = "string",
                                                                 values = new List<ValueResource>
                                                                              {
                                                                                  new ValueResource(1)
                                                                                      {
                                                                                          textDisplayValue =
                                                                                              "ramen noodles"
                                                                                      }
                                                                              }
                                                             }
                                                     }
                                   };

            reportReturnResource.ReportResults.Add(reportResult);

            this.FacadeService
                .GetOrdersBySupplierReport(Arg.Any<OrdersBySupplierSearchResource>(), Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<ReportReturnResource>(reportReturnResource));

            this.Response = this.Client.Get(
                $"/purchasing/reports/orders-by-supplier/report?id={16622}&fromDate={2/11/21}&toDate={2/12/21}",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallFacadeService()
        {
            this.FacadeService.Received().GetOrdersBySupplierReport(
                Arg.Any<OrdersBySupplierSearchResource>(),
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
            resource.ReportResults.First().title.displayString.Should().Be("potat");
            resource.ReportResults.First().results.First().values.First().textDisplayValue.Should().Be("ramen noodles");
        }
    }
}
