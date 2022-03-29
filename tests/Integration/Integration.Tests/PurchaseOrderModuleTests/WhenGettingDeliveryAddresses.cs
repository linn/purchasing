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

    public class WhenGettingDeliveryAddresses : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.DeliveryAddressService.GetAll().Returns(
                new SuccessResult<IEnumerable<LinnDeliveryAddressResource>>(
                    new[] { new LinnDeliveryAddressResource { Description = "D1" }, new LinnDeliveryAddressResource { Description = "D2" } }));

            this.Response = this.Client.Get(
                "/purchasing/purchase-orders/delivery-addresses",
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
            var resources = this.Response.DeserializeBody<IEnumerable<LinnDeliveryAddressResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);

            resources?.First().Description.Should().Be("D1");
            resources.Second().Description.Should().Be("D2");
        }
    }
}
