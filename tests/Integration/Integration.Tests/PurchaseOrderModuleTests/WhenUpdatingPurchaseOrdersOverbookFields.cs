namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingPurchaseOrdersOverbookFields : ContextBase
    {
        private PurchaseOrderResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new PurchaseOrderResource
            {
                OrderNumber = 600179,
                Overbook = "Y",
                OverbookQty = 1,
                CurrentlyUsingOverbookForm = true
            };

            this.MockPurchaseOrderRepository.FindById(600179).Returns(
                new PurchaseOrder
                {
                    OrderNumber = 600179,
                    OverbookQty = 1,
                    Supplier = new Supplier { SupplierId = 1224 }
                });

            this.Response = this.Client.PutAsJsonAsync(
                $"/purchasing/purchase-orders/600179",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallUpdate()
        {
            this.MockDomainService.Received()
                .AllowOverbook(
                    Arg.Any<PurchaseOrder>(),
                    "Y",
                    1m,
                    Arg.Any<IEnumerable<string>>());
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
            var resultResource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resultResource.Should().NotBeNull();
        }
    }
}
