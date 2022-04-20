namespace Linn.Purchasing.Integration.Tests.SigningLimitModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingASigningLimit : ContextBase
    {
        private SigningLimitResource resource;

        private int userNumber;

        [SetUp]
        public void SetUp()
        {
            this.userNumber = 123;

            this.resource = new SigningLimitResource { UserNumber = this.userNumber, ProductionLimit = 123.45m };

            this.FacadeService.Update(
                    this.userNumber,
                    Arg.Is<SigningLimitResource>(a => a.UserNumber == this.userNumber),
                    Arg.Any<List<string>>(),
                    Arg.Any<int>())
                .Returns(
                    new SuccessResult<SigningLimitResource>(
                        new SigningLimitResource
                            {
                                UserNumber = this.userNumber,
                                ProductionLimit = this.resource.ProductionLimit,
                                Links = new[] { new LinkResource("self", $"/purchasing/signing-limits/{this.userNumber}") }
                            }));

            this.Response = this.Client.PutAsJsonAsync(
                $"/purchasing/signing-limits/{this.userNumber}",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallUpdate()
        {
            this.FacadeService.Received()
                .Update(
                    this.userNumber,
                    Arg.Is<SigningLimitResource>(a => a.UserNumber == this.userNumber),
                    Arg.Any<List<string>>(),
                    Arg.Any<int>());
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
            var resultResource = this.Response.DeserializeBody<SigningLimitResource>();
            resultResource.Should().NotBeNull();

            resultResource.ProductionLimit.Should().Be(123.45m);
        }
    }
}
