namespace Linn.Purchasing.Integration.Tests.ThingModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingAThing : ContextBase
    {
        private ThingResource thingResource;

        [SetUp]
        public void SetUp()
        {
            this.thingResource = new ThingResource { Id = 123, Name = "new" };

            this.FacadeService.Add(Arg.Any<ThingResource>()).Returns(
                new CreatedResult<ThingResource>(
                    new ThingResource
                        {
                            Id = 123, Name = "new", Links = new[] { new LinkResource("self", "/purchasing/things/123") }
                        }));

            this.Response = this.Client.Post(
                "/purchasing/things",
                this.thingResource,
                with =>
                    {
                        with.Accept("application/json");
                }).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public void ShouldReturnLocationHeader()
        {
            this.Response.Headers.Location.Should().NotBeNull();
            this.Response.Headers.Location.Should().Be("/purchasing/things/123");
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
            var resources = this.Response.DeserializeBody<ThingResource>();
            resources.Should().NotBeNull();

            resources.Id.Should().Be(123);
            resources.Name.Should().Be("new");
        }
    }
}
