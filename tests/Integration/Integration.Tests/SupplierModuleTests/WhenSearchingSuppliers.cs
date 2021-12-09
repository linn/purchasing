namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingSuppliers : ContextBase
    {
        private string partNumberSearch;

        private string supplierNameSearch;

        private List<SupplierResource> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.partNumberSearch = "PART";
            this.supplierNameSearch = "SUPPLIER";

            this.dataResult = new List<SupplierResource>
                                  {
                                      new SupplierResource
                                          {
                                              Name = "SUPPLIER", Id = 1
                                          }
                                  };

            this.SupplierFacadeService.Search("SUP")
                .Returns(new SuccessResult<IEnumerable<SupplierResource>>(this.dataResult));

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
            var resources = this.Response.DeserializeBody<IEnumerable<SupplierResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);

            resources?.First().Name.Should().Be("SUPPLIER");
        }
    }
}