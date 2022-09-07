namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPuttingDeliveriesForOrderLine : ContextBase
    {
        private PurchaseOrderDeliveryResource[] resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new List<PurchaseOrderDeliveryResource>
                                {
                                    new PurchaseOrderDeliveryResource
                                        {
                                            OrderNumber = 123456,
                                            OrderLine = 1,
                                            DateRequested = 1.January(2000).ToString("o"),
                                            DateAdvised = 1.January(2000).ToString("o")
                                        }
                                }.ToArray();

            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/purchase-orders/deliveries/123456/1",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.MockDomainService.Received().UpdateDeliveriesForOrderLine(
                123456,
                1,
                Arg.Any<IEnumerable<PurchaseOrderDelivery>>(),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldCommit()
        {
            this.MockTransactionManager.Received().Commit();
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            var response = this.Response;

            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PurchaseOrderDeliveryResource[]>();
            resultResource.First().DateAdvised.Should().Be(this.resource.First().DateAdvised);
        }
    }
}
