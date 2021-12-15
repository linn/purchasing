namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
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

    public class WhenSearchingTariffs : ContextBase
    {
        private string searchTerm;

        private List<TariffResource> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.searchTerm = "T1";

            this.dataResult = new List<TariffResource>
                                  {
                                      new TariffResource
                                          {
                                              Code = "T1", Id = 1
                                          }
                                  };

            this.TariffService.Search(this.searchTerm)
                .Returns(new SuccessResult<IEnumerable<TariffResource>>(this.dataResult));

            this.Response = this.Client.Get(
                $"/purchasing/purchase-orders/tariffs?searchTerm={this.searchTerm}",
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
            var resources = this.Response.DeserializeBody<IEnumerable<TariffResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);

            resources?.First().Code.Should().Be("T1");
        }
    }
}
