 namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private string supplierSearchTerm;

        private string orderSearchTerm;

        private bool includeAcknowledged;

        [SetUp]
        public void SetUp()
        {
            this.supplierSearchTerm = "SUPP";
            this.orderSearchTerm = "12345";
            this.includeAcknowledged = true;

            var data = new List<PurchaseOrderDelivery> 
                           { 
                               new PurchaseOrderDelivery 
                                   { 
                                       OrderNumber = 123456, 
                                       PurchaseOrderDetail = new PurchaseOrderDetail()
                                   }
                           };

            this.MockDomainService.SearchDeliveries(
                this.supplierSearchTerm, this.orderSearchTerm, this.includeAcknowledged, null).Returns(data);

            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders/deliveries?" 
                + $"supplierSearchTerm={this.supplierSearchTerm}" 
                + $"&orderNumberSearchTerm={this.orderSearchTerm}"
                + $"&includeAcknowledged={this.includeAcknowledged}",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldPassCorrectParamsToDomain()
        {
            this.MockDomainService.Received().SearchDeliveries(
                this.supplierSearchTerm,
                this.orderSearchTerm,
                this.includeAcknowledged,
                null);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resources = this.Response.DeserializeBody<IEnumerable<PurchaseOrderDeliveryResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);
            resources.First().OrderNumber.Should().Be(123456);
        }
    }
}
