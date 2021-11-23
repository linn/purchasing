namespace Linn.Purchasing.Integration.Tests.ThingModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingAThing : ContextBase
    {
        private int thingId;

        private ThingResource thing;

        [SetUp]
        public void SetUp()
        {
            this.thingId = 1;
            this.thing = new ThingResource { Id = this.thingId };

            this.FacadeService.GetById(this.thingId, Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<ThingResource>(this.thing));

            this.Response = this.Client.Get(
                $"/purchasing/things/{this.thingId}",
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
            var resource = this.Response.DeserializeBody<ThingResource>();
            resource.Should().NotBeNull();

            resource.Id.Should().Be(this.thingId);
        }
    }
}
