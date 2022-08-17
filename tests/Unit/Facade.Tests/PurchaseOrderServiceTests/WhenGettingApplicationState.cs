namespace Linn.Purchasing.Facade.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingApplicationState : ContextBase
    {
        private IResult<PurchaseOrderResource> result;
        [SetUp]
        public void SetUp()
        {
            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>()).Returns(true);
            this.result = this.Sut.GetApplicationState(new List<string>());
        }

        [Test]
        public void ShouldBuildCorrectResourceWithLinks()
        {
            this.result.Should().BeOfType<SuccessResult<PurchaseOrderResource>>();
            var dataResult = ((SuccessResult<PurchaseOrderResource>)this.result).Data;
            dataResult.Links.Length.Should().Be(1);
            dataResult.Links.First().Rel.Should().Be("allow-over-book-search");
            dataResult.Links.First().Href.Should().Be("/purchasing/purchase-orders/allow-over-book");
        }
    }
}