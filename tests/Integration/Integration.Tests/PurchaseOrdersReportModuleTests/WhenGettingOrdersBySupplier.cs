namespace Linn.Purchasing.Integration.Tests.PurchaseOrderReportModuleTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;
    using System.Web;

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
            var resource = new OrdersBySupplierSearchResource() { From = "2/11/21", To = "2/12/21" };

            var builder = new UriBuilder("http://localhost:51699/purchasing/reports/orders-by-supplier/report");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["fromDate"] = "2/11/21";
            query["toDate"] = "2/12/21";
            query["id"] = "16622";
            builder.Query = query.ToString();
            string url = builder.ToString();

            this.Response = this.Client.Get(
                url,
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldCallFacadeService()
        {
            this.FacadeService.Received().GetOrdersBySupplierReport(Arg.Any<OrdersBySupplierSearchResource>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<SuccessResult<ResultsModel>>();
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
