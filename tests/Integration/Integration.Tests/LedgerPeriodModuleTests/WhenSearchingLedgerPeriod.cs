namespace Linn.Purchasing.Integration.Tests.LedgerPeriodModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingLedgerPeriod : ContextBase
    {
        private string monthNameSearch;

        private List<LedgerPeriod> dataResult;
        [SetUp]
        public void SetUp()
        {
            this.monthNameSearch = "FEB";

            this.dataResult = new List<LedgerPeriod>
                                  {
                                      new LedgerPeriod { MonthName = "FEB1995", PeriodNumber = 20 }
                                  };

            this.LedgerPeriodRepository.FilterBy(Arg.Any<Expression<Func<LedgerPeriod, bool>>>())
                .Returns(this.dataResult.AsQueryable());

            this.Response = this.Client.Get(
                $"/purchasing/ledger-periods/search?searchTerm={this.monthNameSearch}",
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
            var resources = this.Response.DeserializeBody<IEnumerable<LedgerPeriod>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(1);
            resources.Should().Contain(a => a.PeriodNumber == 20);
        }
    }
}

