namespace Linn.Purchasing.Integration.Tests.BomStandardPriceModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPostingUpdate : ContextBase
    {
        private BomStandardPricesResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new BomStandardPricesResource
                                {
                                    Lines = new List<BomStandardPrice>
                                                {
                                                    new BomStandardPrice { BomName = "BOM BOM" },
                                                },
                                    Remarks = "REMARKABLE"
                                };
            this.DomainService.SetStandardPrices(
                    Arg.Any<IEnumerable<BomStandardPrice>>(), Arg.Any<int>(), this.resource.Remarks)
                .Returns(new SetStandardPriceResult
                             {
                                 ReqNumber = 123,
                                 Lines = this.resource.Lines,
                                 Message = "HELLO"
                             });
            this.Response = this.Client
                .PostAsJsonAsync("/purchasing/boms/prices", this.resource).Result;
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnUpdated()
        {
            var resultResource = this.Response.DeserializeBody<BomStandardPricesResource>();
            resultResource.Should().NotBeNull();

            resultResource.ReqNumber.Should().Be(123);
        }
    }
}
