namespace Linn.Purchasing.Integration.Tests.PurchaseOrderReportModuleTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingOrdersByPartExport : ContextBase
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
                .GetOrdersByPartExport(Arg.Any<OrdersByPartSearchResource>(), Arg.Any<IEnumerable<string>>())
                .Returns(new MemoryStream());

            this.Response = this.Client.Get(
                $"/purchasing/reports/orders-by-supplier/export?partNumber={"MCP 123"}&fromDate={(1.January(2022).ToLongDateString())}&toDate={(1.February(2022).ToLongDateString())}&cancelled=N",
                with => { with.Accept("text/csv"); }).Result;
        }

        [Test]
        public void ShouldCallFacadeService()
        {
            this.FacadeService.Received().GetOrdersByPartExport(
                Arg.Any<OrdersByPartSearchResource>(),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnCSVContentType()
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
