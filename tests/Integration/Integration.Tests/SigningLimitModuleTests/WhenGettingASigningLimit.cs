namespace Linn.Purchasing.Integration.Tests.SigningLimitModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingASigningLimit : ContextBase
    {
        private int userNumber;

        private SigningLimitResource signingLimit;

        [SetUp]
        public void SetUp()
        {
            this.userNumber = 1;
            this.signingLimit = new SigningLimitResource { UserNumber = this.userNumber };

            this.FacadeService.GetById(this.userNumber, Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<SigningLimitResource>(this.signingLimit));

            this.Response = this.Client.Get(
                $"/purchasing/signing-limits/{this.userNumber}",
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
            var resource = this.Response.DeserializeBody<SigningLimitResource>();
            resource.Should().NotBeNull();

            resource.UserNumber.Should().Be(this.userNumber);
        }
    }
}
