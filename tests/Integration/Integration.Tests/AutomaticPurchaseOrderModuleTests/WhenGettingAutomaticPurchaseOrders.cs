namespace Linn.Purchasing.Integration.Tests.AutomaticPurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingAutomaticPurchaseOrders : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.FacadeService.GetAll().Returns(
                new SuccessResult<IEnumerable<AutomaticPurchaseOrderResource>>(
                    new[]
                        {
                            new AutomaticPurchaseOrderResource { Id = 1 }, new AutomaticPurchaseOrderResource { Id = 2 }
                        }));

            this.Response = this.Client.Get(
                "/purchasing/automatic-purchase-orders",
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
            var resources = this.Response.DeserializeBody<IEnumerable<AutomaticPurchaseOrderResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);
            resources.Should().Contain(a => a.Id == 1);
            resources.Should().Contain(a => a.Id == 2);
        }
    }
}
