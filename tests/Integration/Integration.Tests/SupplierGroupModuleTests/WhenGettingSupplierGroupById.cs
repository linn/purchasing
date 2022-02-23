namespace Linn.Purchasing.Integration.Tests.SupplierGroupModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSupplierGroupById : ContextBase
    {
        private int id;

        private SupplierGroupResource supplierGroup;

        [SetUp]
        public void SetUp()
        {
            this.id = 1;
            this.supplierGroup = new SupplierGroupResource { Id = 1, Name = "s g 1" };

            this.SupplierGroupFacadeService.GetById(1, Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<SupplierGroupResource>(this.supplierGroup));

            this.Response = this.Client.Get(
                $"/purchasing/supplier-groups/{this.id}",
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
            this.SupplierGroupFacadeService.Received().GetById(1, Arg.Any<IEnumerable<string>>());
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
            var resource = this.Response.DeserializeBody<SupplierGroupResource>();
            resource.Id.Should().Be(this.supplierGroup.Id);
            resource.Name.Should().Be(this.supplierGroup.Name);
        }
    }
}
