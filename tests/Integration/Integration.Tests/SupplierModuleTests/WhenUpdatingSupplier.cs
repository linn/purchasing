namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingSupplier : ContextBase
    {
        private SupplierResource resource;

        private Supplier supplier;

        [SetUp]
        public void SetUp()
        {

            this.resource = new SupplierResource { Id = 1, Name = "NEW NAME" };
            this.supplier = new Supplier { SupplierId = 1, Name = "SUPPLIER" };

            this.MockSupplierRepository.FindById(1).Returns(this.supplier);
            this.Response = this.Client.Put(
                $"/purchasing/suppliers/{this.resource.Id}",
                this.resource,
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
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
        public void ShouldCallDomainService()
        {
            this.MockDomainService.Received().UpdateSupplier(
                Arg.Is<Supplier>(s => s.SupplierId == 1  && s.Name == "SUPPLIER"),
                Arg.Is<Supplier>(s => s.SupplierId == 1 && s.Name == "NEW NAME"),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<SupplierResource>();
            resultResource.Should().NotBeNull();
            resultResource.Id.Should().Be(1);
        }
    }
}
