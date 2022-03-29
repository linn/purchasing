using Linn.Common.Facade;

namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPurchaseOrder : ContextBase
    {
        private PurchaseOrder req;
        private int orderNumberSearch;

        [SetUp]
        public void SetUp()
        {
            this.orderNumberSearch = 600179;
            this.req = new PurchaseOrder()
                           {
                               OrderNumber = 600179,
                               Cancelled = string.Empty,
                               DocumentType = string.Empty,
                               OrderDate = 10.January(2021),
                               Overbook = string.Empty,
                               OverbookQty = 1,
                               SupplierId = 1224
            };

            this.PurchaseOrderFacadeService.GetById(this.orderNumberSearch, Arg.Any<IEnumerable<string>>())
                .ReturnsForAnyArgs(
                    new SuccessResult<PurchaseOrderResource>(
                        new PurchaseOrderResource
                        {
                            OrderNumber = 600179,
                            OverbookQty = 1,
                            SupplierId = 1224
                        }));

            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders/{this.orderNumberSearch}",
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
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resource.OrderNumber.Should().Be(this.req.OrderNumber);
            resource.OverbookQty.Should().Be(this.req.OverbookQty);
            resource.SupplierId.Should().Be(this.req.SupplierId);
        }
    }
}
