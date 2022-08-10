namespace Linn.Purchasing.Integration.Tests.LedgerPeriodModuleTests
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

    public class WhenGettingLedgerPeriods : ContextBase
    {
        private List<LedgerPeriodResource> results;

        [SetUp]
        public void SetUp()
        {
            this.results = new List<LedgerPeriodResource>
                                  {
                                      new LedgerPeriodResource { PeriodNumber = 1 },
                                      new LedgerPeriodResource { PeriodNumber = 2 }
                                  };

            this.LedgerPeriodFacadeService.GetAll()
                .Returns(new SuccessResult<IEnumerable<LedgerPeriodResource>>(this.results));

            this.Response = this.Client.Get(
                $"/purchasing/ledger-periods",
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
            var resources = this.Response.DeserializeBody<IEnumerable<LedgerPeriodResource>>().ToList();
            resources.Should().HaveCount(2);
            resources.Should().Contain(a => a.PeriodNumber == 1);
            resources.Should().Contain(a => a.PeriodNumber == 2);
        }
    }
}
