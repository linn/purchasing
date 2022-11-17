namespace Linn.Purchasing.Integration.Tests.BomTypeChangeModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPostingBomTypeChange : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var resource = new BomTypeChangeResource { PartNumber = "TEST 1", OldBomType = "A", NewBomType = "C" };
            var part = new Part { PartNumber = "TEST 1", Currency = new Currency { Name = "Pebbles" } };
            this.PartService.ChangeBomType(Arg.Any<BomTypeChange>(), Arg.Any<IEnumerable<string>>()).Returns(part);
            this.Response = this.Client.PostAsJsonAsync("/purchasing/bom-type-change", resource).Result;
        }

        [Test]
        public void ShouldCallUpdate()
        {
            this.PartService.Received().ChangeBomType(
                Arg.Any<BomTypeChange>(),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<BomTypeChangeResource>();
            resultResource.Should().NotBeNull();
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
