namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingStateAndUserHasPermissionToCreateOrdersForOtherUsers : ContextBase
    {

        [SetUp]
        public void SetUp()
        {
            this.MockAuthService
                .HasPermissionFor(
                    AuthorisedAction.PurchaseOrderCreateForOtherUser, 
                    Arg.Any<IEnumerable<string>>()).Returns(true);

            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders/application-state",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldBuildCreateLinks()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resource.Links.Any(l => l.Rel == "create-for-other-user").Should().BeTrue();
        }
    }
}
