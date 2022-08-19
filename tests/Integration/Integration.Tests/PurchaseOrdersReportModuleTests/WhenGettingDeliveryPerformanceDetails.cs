namespace Linn.Purchasing.Integration.Tests.PurchaseOrdersReportModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDeliveryPerformanceDetails : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var reportReturnResource = new ReportReturnResource();
            var reportResult = new ReportResultResource
                                   {
                                       title = new DisplayResource("DPD")
                                   };

            reportReturnResource.ReportResults.Add(reportResult);

            this.FacadeService
                .GetDeliveryPerformanceDetailReport(Arg.Any<DeliveryPerformanceRequestResource>())
                .Returns(new SuccessResult<ReportReturnResource>(reportReturnResource));

            this.Response = this.Client.Get(
                "/purchasing/reports/delivery-performance-details/report?startPeriod=2&endPeriod=4&supplierId=123",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallFacadeService()
        {
            this.FacadeService.Received().GetDeliveryPerformanceDetailReport(
                Arg.Is<DeliveryPerformanceRequestResource>(
                    a => a.StartPeriod == 2 && a.EndPeriod == 4 & a.SupplierId == 123));
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
            resource.ReportResults.First().title.displayString.Should().Be("DPD");
        }
    }
}
