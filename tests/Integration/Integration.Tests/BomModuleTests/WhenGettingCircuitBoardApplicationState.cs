namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;

    using NUnit.Framework;

    public class WhenGettingCircuitBoardApplicationState : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Response = this.Client.Get(
                $"/purchasing/boms/boards/application-state",
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
            var resource = this.Response.DeserializeBody<CircuitBoardResource>();
            resource.Should().NotBeNull();
            resource.Links.Length.Should().Be(0);
        }
    }
}
