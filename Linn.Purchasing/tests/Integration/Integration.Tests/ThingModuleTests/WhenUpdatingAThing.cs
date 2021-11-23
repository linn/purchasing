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

    public class WhenUpdatingAThing : ContextBase
    {
        private ThingResource resource;

        private int thingId;

        [SetUp]
        public void SetUp()
        {
            this.thingId = 123;

            this.resource = new ThingResource { Id = this.thingId, Name = "new name" };

            this.FacadeService.Update(this.thingId, Arg.Is<ThingResource>(a => a.Id == this.thingId))
                .Returns(
                    new SuccessResult<ThingResource>(
                        new ThingResource
                            {
                                Id = this.thingId,
                                Name = this.resource.Name,
                                Links = new[] { new LinkResource("self", $"/template/things/{this.thingId}") }
                            }));

            this.Response = this.Client.Put(
                $"/template/things/{this.thingId}",
                this.resource,
                with =>
                    {
                        with.Accept("application/json");
                }).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallUpdate()
        {
            this.FacadeService.Received().Update(this.thingId, Arg.Is<ThingResource>(a => a.Id == this.thingId));
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
            var resultResource = this.Response.DeserializeBody<ThingResource>();
            resultResource.Should().NotBeNull();

            resultResource.Name.Should().Be("new name");
        }
    }
}
