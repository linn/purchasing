namespace Linn.Purchasing.Integration.Tests.BomStandardPriceModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private IEnumerable<BomStandardPrice> data;

        [SetUp]
        public void SetUp()
        {
            this.data = new List<BomStandardPrice>
                            {
                                new BomStandardPrice { BomName = "BOM1" },
                                new BomStandardPrice { BomName = "BOM1" }
                            };
            this.DomainService.GetPriceVarianceInfo("BOM")
                .Returns(this.data.AsQueryable());

            this.Response = this.Client.Get(
                "/purchasing/boms/prices?searchTerm=BOM",
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
            this.Response.Content.Headers.ContentType.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<BomStandardPricesResource>();
            resource.Should().NotBeNull();
            resource.Lines.Should().HaveCount(2);
        }
    }
}
