namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingSuppliers : ContextBase
    {
        private string supplierNameSearch;

        private List<Supplier> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.supplierNameSearch = "SUPP";

            this.dataResult = new List<Supplier>
                                  {
                                      new Supplier
                                          {
                                              SupplierId = 1,
                                              Name = "SUPPLIER",
                                              OpenedBy = new Employee { Id = 1 }
                                          }
        };

            this.MockSupplierRepository.FilterBy(Arg.Any<Expression<Func<Supplier, bool>>>())
                .Returns(this.dataResult.AsQueryable());

            this.Response = this.Client.Get(
                $"/purchasing/suppliers?searchTerm={this.supplierNameSearch}",
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
            var resources = this.Response.DeserializeBody<IEnumerable<SupplierResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);
            resources?.First().Name.Should().Be("SUPPLIER");
        }

        [Test]
        public void ShouldBuildLinks()
        {
            var resources = this.Response.DeserializeBody<IEnumerable<SupplierResource>>()?.ToArray();
            resources?.First().Links.Single(x => x.Rel == "self").Href.Should()
                .Be($"/purchasing/suppliers/{this.dataResult.First().SupplierId}");
        }
    }
}
