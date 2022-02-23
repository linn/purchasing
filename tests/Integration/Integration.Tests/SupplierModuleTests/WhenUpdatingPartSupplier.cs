namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingPartSupplier : ContextBase
    {
        private PartSupplierResource updateResource;

        [SetUp]
        public void SetUp()
        {
            this.updateResource = new PartSupplierResource
                                      {
                                          PartNumber = "PART",
                                          SupplierId = 100
                                      };

            this.MockPartSupplierRepository
                .FindById(Arg.Is<PartSupplierKey>(
                    k => k.PartNumber == this.updateResource.PartNumber && k.SupplierId == this.updateResource.SupplierId))
                .Returns(
                    new PartSupplier
                        {
                            PartNumber = this.updateResource.PartNumber, SupplierId = this.updateResource.SupplierId,
                            Part = new Part { PartNumber = this.updateResource.PartNumber },
                            Supplier = new Supplier { SupplierId = this.updateResource.SupplierId }
                        });

            this.Response = this.Client.Put(
                $"/purchasing/part-suppliers/record?partId={1}&supplierId={100}",
                this.updateResource,
                with =>
                {
                    with.Accept("application/json");
                }).Result;
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
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PartSupplierResource>();
            resultResource.Should().NotBeNull();
        }
    }
}
