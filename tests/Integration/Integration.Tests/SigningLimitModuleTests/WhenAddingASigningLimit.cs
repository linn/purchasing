namespace Linn.Purchasing.Integration.Tests.SigningLimitModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingASigningLimit : ContextBase
    {
        private SigningLimitResource signingLimitResource;

        [SetUp]
        public void SetUp()
        {
            this.signingLimitResource = new SigningLimitResource { UserNumber = 4 };

            this.FacadeService.Add(Arg.Any<SigningLimitResource>(), Arg.Any<List<string>>(), Arg.Any<int>())
                .Returns(new CreatedResult<SigningLimitResource>(
                    new SigningLimitResource
                        {
                            UserNumber = 4
                        }));

            this.Response = this.Client.PostAsJsonAsync(
                "/purchasing/signing-limits",
                this.signingLimitResource).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
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
            var resources = this.Response.DeserializeBody<SigningLimitResource>();
            resources.Should().NotBeNull();

            resources.UserNumber.Should().Be(4);
        }
    }
}
