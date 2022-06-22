namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingDeliveriesForAnOrderLine : ContextBase
    {
        private PurchaseOrderDelivery[] data;

        [SetUp]
        public void SetUp()
        {
            this.data = new List<PurchaseOrderDelivery>
                                {
                                    new PurchaseOrderDelivery
                                        {
                                            OrderNumber = 123456,
                                            OrderLine = 1,
                                            DateAdvised = 1.January(2000),
                                            PurchaseOrderDetail = new PurchaseOrderDetail()
                                        }
                                }.ToArray();
            this.MockDomainService.SearchDeliveries(null, "123456", true, 1).Returns(this.data);
            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders/deliveries/123456/1/",
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
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
