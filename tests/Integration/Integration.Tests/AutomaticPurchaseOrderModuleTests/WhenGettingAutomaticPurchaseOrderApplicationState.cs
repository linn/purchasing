namespace Linn.Purchasing.Integration.Tests.AutomaticPurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingAutomaticPurchaseOrderApplicationState : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.FacadeService.GetApplicationState(Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<AutomaticPurchaseOrderResource>(new AutomaticPurchaseOrderResource()));

            this.Response = this.Client.Get(
                $"/purchasing/automatic-purchase-orders/application-state",
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
            var resource = this.Response.DeserializeBody<AutomaticPurchaseOrderResource>();
            resource.Should().NotBeNull();
        }
    }
}
