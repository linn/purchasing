namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingPartSupplier : ContextBase
    {
        private PartSupplierResource resource;

        private PartSupplier partSupplier;

        [SetUp]
        public void SetUp()
        {

            this.resource = new PartSupplierResource
            {
                PartNumber = "PART",
                SupplierId = 100
            };

            this.partSupplier = new PartSupplier
                                    {
                                        PartNumber = "PART",
                                        SupplierId = 100,
                                        Part = new Part { PartNumber = "PART" },
                                        Supplier = new Supplier { SupplierId = 100 }
                                    };

            this.Response = this.Client.Post(
                $"/purchasing/part-suppliers/record",
                this.resource,
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
