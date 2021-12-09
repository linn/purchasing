namespace Linn.Purchasing.Integration.Tests.SigningLimitModuleTests
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

    public class WhenGettingSigningLimits : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.FacadeService.GetAll().Returns(
                new SuccessResult<IEnumerable<SigningLimitResource>>(
                    new[] { new SigningLimitResource { UserNumber = 1 }, new SigningLimitResource { UserNumber = 2 } }));

            this.Response = this.Client.Get(
                "/purchasing/signing-limits",
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
            var resources = this.Response.DeserializeBody<IEnumerable<SigningLimitResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);
            resources.Should().Contain(a => a.UserNumber == 1);
            resources.Should().Contain(a => a.UserNumber == 2);
        }
    }
}
