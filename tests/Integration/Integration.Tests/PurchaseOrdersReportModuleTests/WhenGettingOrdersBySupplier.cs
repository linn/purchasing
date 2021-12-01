namespace Linn.Purchasing.Integration.Tests.PurchaseOrderReportModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingOrdersBySupplier : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.FacadeService.GetOrdersBySupplierReport(Arg.Any<OrdersBySupplierSearchResource>()).Returns(
                new SuccessResult<ResultsModel>(new ResultsModel()));

            this.Response = this.Client.Get(
                "/purchasing/reports/orders-by-supplier/118",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldCallFacadeService()
        {
            this.FacadeService.Received().GetOrdersBySupplierReport(Arg.Any<OrdersBySupplierSearchResource>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<ResultsModel>();
            resource.Should().NotBeNull();
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
    }
}
