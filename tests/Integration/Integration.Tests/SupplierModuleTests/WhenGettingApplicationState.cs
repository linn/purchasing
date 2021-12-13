namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingApplicationState : ContextBase
    {
        private PartSupplierResource partSupplierResource;

        [SetUp]
        public void SetUp()
        {
            this.partSupplierResource = new PartSupplierResource
                                            {
                                                Links = new LinkResource[1] { new LinkResource("edit", "/edit") }
                                            };

            this.PartSupplierFacadeService.GetApplicationState(Arg.Any<List<string>>())
                .Returns(new SuccessResult<PartSupplierResource>(this.partSupplierResource));


            this.Response = this.Client.Get(
                $"/purchasing/part-suppliers/application-state",
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
            var resource = this.Response.DeserializeBody<PartSupplierResource>();
            resource.Should().NotBeNull();
            resource.Links.Length.Should().Be(1);
            resource.Links.First().Rel.Should().Be("edit");
            resource.Links.First().Href.Should().Be("/edit");
        }
    }
}
