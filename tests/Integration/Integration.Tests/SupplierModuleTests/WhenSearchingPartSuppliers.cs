namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingPartSuppliers : ContextBase
    {
        private string partNumberSearch;

        private string supplierNameSearch;

        private PartSupplier partSupplier;

        [SetUp]
        public void SetUp()
        {
            this.partNumberSearch = "PART";
            this.supplierNameSearch = "SUPPLIER";

            this.partSupplier = new PartSupplier
                                    {
                                        PartNumber = "PART",
                                        SupplierId = 100,
                                        Part = new Part { PartNumber = "PART" },
                                        Supplier = new Supplier { SupplierId = 100 }
                                    };

            this.MockPartSupplierRepository.FilterBy(Arg.Any<Expression<Func<PartSupplier, bool>>>())
                .Returns(new List<PartSupplier>
                             {
                                this.partSupplier
                             }.AsQueryable());
            
            this.Response = this.Client.Get(
                $"/purchasing/part-suppliers?partNumber={this.partNumberSearch}&supplierName={this.supplierNameSearch}",
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
            var resources = this.Response.DeserializeBody<IEnumerable<PartSupplierResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);

            resources?.First().PartNumber.Should().Be("PART");
        }
    }
}
