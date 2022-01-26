namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPartPriceConversions : ContextBase
    {
        private PartPriceConversionsResource resource;

        [SetUp]
        public void SetUp()
        {
            
            this.resource = new PartPriceConversionsResource
            {
                BaseNewPrice = 100,
                NewPrice = 100
            };

            this.PartFacadeService.GetPrices("PART", "USD", 100m)
                .Returns(new SuccessResult<PartPriceConversionsResource>(this.resource));


            this.Response = this.Client.Get(
                $"/purchasing/part-suppliers/part-price-conversions?partNumber=PART&newCurrency=USD&newPrice=100",
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
            var resource = this.Response.DeserializeBody<PartPriceConversionsResource>();
            resource.Should().NotBeNull();
            resource.NewPrice.Should().Be(100m);
        }
    }
}
