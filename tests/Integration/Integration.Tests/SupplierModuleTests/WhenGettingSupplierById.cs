namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSupplierById : ContextBase
    {
        private int id;

        private Supplier supplier;

        [SetUp]
        public void SetUp()
        {
            this.id = 1;
            this.supplier = new Supplier { SupplierId = 1, Name = "SUPPLIER" };

            this.SupplierRepository.FindById(1).Returns(this.supplier);

            this.Response = this.Client.Get(
                $"/purchasing/suppliers/{this.id}",
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
            resource.Id.Should().Be(this.supplier.SupplierId);
            resource.Name.Should().Be(this.supplier.Name);
        }

        [Test]
        public void ShouldBuildLinks()
        {
            var resource = this.Response.DeserializeBody<SupplierResource>();
            resource.Links.Single(x => x.Rel == "self").Href.Should()
                .Be($"/purchasing/suppliers/{this.supplier.SupplierId}");
        }
    }
}
