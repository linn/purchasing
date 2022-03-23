namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPOReqApplicationState : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockAuthService.HasPermissionFor("purchase-order-req.create", Arg.Any<IEnumerable<string>>()).Returns(true);

            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders/reqs/application-state",
            with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldBuildLinks()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderReqResource>();
            resource.Links.Single(x => x.Rel == "create").Href.Should().Be("/purchasing/purchase-orders/reqs/create");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
