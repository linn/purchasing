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

    public class WhenGettingCurrencies : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.CurrencyService.GetAll().Returns(
                new SuccessResult<IEnumerable<CurrencyResource>>(
                    new[] { new CurrencyResource { Code = "GBP" }, new CurrencyResource { Code = "USD" } }));

            this.Response = this.Client.Get(
                "/purchasing/purchase-orders/currencies",
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
            var resources = this.Response.DeserializeBody<IEnumerable<CurrencyResource>>().ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);

            resources.First().Code.Should().Be("GBP");
            resources.ElementAt(1).Code.Should().Be("USD");
        }
    }
}
