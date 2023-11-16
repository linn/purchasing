namespace Linn.Purchasing.Integration.Tests.VendorManagerModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingVendorManagerById : ContextBase
    {
        private string id;

        private VendorManagerResource vendorManager;

        [SetUp]
        public void SetUp()
        {
            this.id = "M";
            this.vendorManager = new VendorManagerResource { VmId = this.id, Name = "vm1" };

            this.VendorManagerFacadeService.GetById(this.id, Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<VendorManagerResource>(this.vendorManager));

            this.Response = this.Client.Get(
                $"/purchasing/vendor-managers/{this.id}",
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
        public void ShouldCallFacadeService()
        {
            this.VendorManagerFacadeService.Received().GetById(this.id, Arg.Any<IEnumerable<string>>());
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
            var resource = this.Response.DeserializeBody<VendorManagerResource>();
            resource.VmId.Should().Be(this.id);
        }
    }
}
