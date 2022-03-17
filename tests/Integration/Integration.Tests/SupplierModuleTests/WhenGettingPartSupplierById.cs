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

    public class WhenGettingPartSupplierById : ContextBase
    {
        private PartSupplierKey key;

        private PartSupplier partSupplier;

        [SetUp]
        public void SetUp()
        {
            this.partSupplier = new PartSupplier
                                    {
                                        PartNumber = "PART",
                                        SupplierId = 100,
                                        Part = new Part { PartNumber = "PART" },
                                        Supplier = new Supplier { SupplierId = 100 }
                                    };
            this.key = new PartSupplierKey
                           {
                               PartNumber = "PART",
                               SupplierId = 100
            };
            
            this.PartFacadeService.GetPartNumberFromId(100).Returns(this.partSupplier.Part.PartNumber);
            this.MockPartSupplierRepository.FindById(Arg.Is<PartSupplierKey>(
                    k => k.PartNumber == this.partSupplier.PartNumber && k.SupplierId == this.partSupplier.SupplierId))
                .Returns(this.partSupplier);

            this.Response = this.Client.Get(
                $"/purchasing/part-suppliers/record?partId={100}&supplierId={100}",
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

            resource.PartNumber.Should().Be(this.key.PartNumber);
        }
    }
}
