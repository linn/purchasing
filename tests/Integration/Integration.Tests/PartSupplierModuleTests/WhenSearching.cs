namespace Linn.Purchasing.Integration.Tests.PartSupplierModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearching : ContextBase
    {
        private string partNumberSearch;

        private string supplierNameSearch;

        private List<PartSupplierResource> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.partNumberSearch = "PART";
            this.supplierNameSearch = "SUPPLIER";

            this.dataResult = new List<PartSupplierResource>
                                  {
                                      new PartSupplierResource
                                          {
                                              PartNumber = "PART", SupplierName = "SUPPLIER", SupplierId = 1
                                          }
                                  };

            this.FacadeService.FilterBy(
                    Arg.Is<PartSupplierSearchResource>(
                        x => 
                x.PartNumberSearchTerm == this.partNumberSearch && x.SupplierNameSearchTerm == this.supplierNameSearch),
                    Arg.Any<IEnumerable<string>>())
                .Returns(new SuccessResult<IEnumerable<PartSupplierResource>>(this.dataResult));

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
