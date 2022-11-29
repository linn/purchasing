namespace Linn.Purchasing.Integration.Tests.BomTypeChangeModuleTests
{
    using System;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBomTypeChange : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var resource = new BomTypeChangeResource { PartNumber = "TEST 1", OldBomType = "A", NewBomType = "C" };
            var part = new Part { Id = 1, PartNumber = "TOAST", Currency = new Currency { Name = "GBP" } };
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(part);
            this.Response = this.Client.Get(
                "/purchasing/bom-type-change/1",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldLookupPart()
        {
            this.PartRepository.Received().FindBy(Arg.Any<Expression<Func<Part, bool>>>());
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
