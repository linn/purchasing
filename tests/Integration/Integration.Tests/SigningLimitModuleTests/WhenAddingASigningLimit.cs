﻿namespace Linn.Purchasing.Integration.Tests.SigningLimitModuleTests
{
    using System.Net;

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

            this.FacadeService.Add(Arg.Any<SigningLimitResource>())
                .Returns(new CreatedResult<SigningLimitResource>(
                    new SigningLimitResource
                        {
                            UserNumber = 4
                        }));

            this.Response = this.Client.Post(
                "/purchasing/signing-limits",
                this.signingLimitResource,
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
