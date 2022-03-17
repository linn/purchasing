namespace Linn.Purchasing.Integration.Tests.ManufacturerModuleTests
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

    public class WhenSearchingManufacturers : ContextBase
    {
        private string searchTerm;

        private List<ManufacturerResource> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.searchTerm = "SUPP";

            this.dataResult = new List<ManufacturerResource>
                                  {
                                      new ManufacturerResource
                                          {
                                             Code = "MAN"
                                          }
                                  };

            this.ManufacturerFacadeService.Search(this.searchTerm)
                .Returns(new SuccessResult<IEnumerable<ManufacturerResource>>(this.dataResult));

            this.Response = this.Client.Get(
                $"/purchasing/manufacturers?searchTerm={this.searchTerm}",
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
            var resources = this.Response.DeserializeBody<IEnumerable<ManufacturerResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);

            resources?.First().Code.Should().Be("MAN");
        }
    }
}
