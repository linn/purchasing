namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSuppliersApplicationState : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.MockAuthService.HasPermissionFor(
                AuthorisedAction.SupplierCreate,
                Arg.Any<IEnumerable<string>>()).Returns(true);
            this.Response = this.Client.Get(
                $"/purchasing/suppliers/application-state",
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
            var resource = this.Response.DeserializeBody<SupplierResource>();
            resource.Should().NotBeNull();
            resource.Links.Length.Should().Be(1);
            resource.Links.First().Rel.Should().Be("create");
            resource.Links.First().Href.Should().Be("purchasing/suppliers/create");
        }
    }
}
