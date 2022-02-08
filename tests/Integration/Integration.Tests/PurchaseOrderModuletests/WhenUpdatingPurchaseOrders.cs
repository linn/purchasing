namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingPurchaseOrders : ContextBase
    {
        private PurchaseOrderResource resource;

        [SetUp]
        public void SetUp()
        {

            this.resource = new PurchaseOrderResource
            {
                OrderNumber = 600179,
                Overbook = "Y",
                OverbookQty = 1
            };

            this.PurchaseOrderFacadeService.Update(Arg.Any<int>(), Arg.Any<PurchaseOrderResource>())
                .ReturnsForAnyArgs(
                    new SuccessResult<PurchaseOrderResource>(
                        new PurchaseOrderResource
                        {
                            OrderNumber = 600179,
                            Overbook = "Y",
                            OverbookQty = 1,
                        }));

            this.Response = this.Client.Put(
                $"/purchasing/purchase-orders/overbook?orderNumber={600179}",
                this.resource,
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
        public void ShouldCallUpdate()
        {
            this.PurchaseOrderFacadeService.Received()
                .Update(Arg.Any<int>(), Arg.Any<PurchaseOrderResource>(), Arg.Any<IEnumerable<string>>());
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
            var resultResource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resultResource.Should().NotBeNull();
        }
    }
}
