namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPurchaseOrdersApplicationState : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            LinkResource[] linkArray =
                {
                    new LinkResource()
                        {
                            Rel = "allow-over-book-search", Href = $"purchasing/purchase-orders/allow-over-book"
                        }
                };
            this.PurchaseOrderFacadeService.GetApplicationState().ReturnsForAnyArgs(
                new SuccessResult<PurchaseOrderResource>(new PurchaseOrderResource { Links = linkArray }));
            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders/application-state",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldBuildLinks()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resource.Links.Single(x => x.Rel == "allow-over-book-search").Href.Should()
                .Be("purchasing/purchase-orders/allow-over-book");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}