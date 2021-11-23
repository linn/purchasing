namespace Linn.Purchasing.Integration.Tests.ThingModuleTests
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

    public class WhenGettingThings : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.FacadeService.GetAll().Returns(
                new SuccessResult<IEnumerable<ThingResource>>(
                    new[] { new ThingResource { Id = 1 }, new ThingResource { Id = 2 } }));

            this.Response = this.Client.Get(
                "/template/things",
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
            var resources = this.Response.DeserializeBody<IEnumerable<ThingResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);

            resources?.First().Id.Should().Be(1);
            resources.Second().Id.Should().Be(2);
        }
    }
}
