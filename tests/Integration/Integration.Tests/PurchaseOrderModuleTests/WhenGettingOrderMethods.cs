namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
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

    public class WhenGettingOrderMethods : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.OrderMethodService.GetAll().Returns(
                new SuccessResult<IEnumerable<OrderMethodResource>>(
                    new[]
                        {
                            new OrderMethodResource { Description = "D1" },
                            new OrderMethodResource { Description = "D2" }
                        }));

            this.Response = this.Client.Get(
                "/purchasing/purchase-orders/methods",
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
            var resources = this.Response.DeserializeBody<IEnumerable<OrderMethodResource>>().ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);

            resources.First().Description.Should().Be("D1");
            resources.ElementAt(1).Description.Should().Be("D2");
        }
    }
}
