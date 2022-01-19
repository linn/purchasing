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

    public class WhenGettingPriceChangeReasons : ContextBase
    {
        private List<PriceChangeReasonResource> dataResult;

        [SetUp]
        public void SetUp()
        {
            this.dataResult = new List<PriceChangeReasonResource>
                                  {
                                      new PriceChangeReasonResource
                                          {
                                              ReasonCode = "CODE"
                                          }
                                  };

            this.PriceChangeReasonService.GetAll()
                .Returns(new SuccessResult<IEnumerable<PriceChangeReasonResource>>(this.dataResult));

            this.Response = this.Client.Get(
                $"/purchasing/price-change-reasons",
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
            var resources = this.Response.DeserializeBody<IEnumerable<PriceChangeReasonResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);

            resources?.First().ReasonCode.Should().Be("CODE");
        }
    }
}
