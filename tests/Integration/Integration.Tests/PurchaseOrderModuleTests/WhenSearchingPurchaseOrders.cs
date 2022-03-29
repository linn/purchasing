namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingPurchaseOrders : ContextBase
    {
        private string orderNumberSearch;

        private List<PurchaseOrderResource> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.orderNumberSearch = "600179";

            this.dataResult = new List<PurchaseOrderResource>
                                  {
                                      new PurchaseOrderResource
                                          {
                                              OrderNumber = 600179,
                                              Cancelled = string.Empty,
                                              DocumentType = string.Empty,
                                              DateOfOrder = 10.January(2021),
                                              Overbook = string.Empty,
                                              OverbookQty = 1,
                                              SupplierId = 1224
                                          }
                                  };

            this.PurchaseOrderFacadeService.Search(
                    this.orderNumberSearch,
                    Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<IEnumerable<PurchaseOrderResource>>(this.dataResult));

            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders?searchTerm={this.orderNumberSearch}",

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
            var resources = this.Response.DeserializeBody<IEnumerable<PurchaseOrderResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);
            resources?.First().OrderNumber.Should().Be(600179);
        }
    }
}
