namespace Linn.Purchasing.Integration.Tests.LedgerPeriodModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    using Org.BouncyCastle.Crypto.Parameters;

    public class WhenSearchingLedgerPeriod : ContextBase
    {

        private IRepository<LedgerPeriod, int> LedgerPeriodRepository;

        [SetUp]
        public void SetUp()
        {
            this.LedgerPeriodRepository = Substitute.For<IRepository<LedgerPeriod, int>>();

            this.LedgerPeriodRepository.FilterBy(Arg.Any<Expression<Func<LedgerPeriod, bool>>>()).Returns(
                new List<LedgerPeriod>
                    {   
                        new LedgerPeriod 
                            {
                                MonthName = "JAN2005",
                                PeriodNumber = 20
                            },
                         new LedgerPeriod
                            {
                                MonthName = "OCT2024",
                                PeriodNumber = 30

                            }
                    }.AsQueryable());

            this.Response = this.Client.Get(
                "/purchasing/ledger-periods?searchTerm=JAN2005",
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
            var resources = this.Response.DeserializeBody<IEnumerable<LedgerPeriodResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);
            resources.Should().Contain(a => a.PeriodNumber == 20);
            resources.Should().Contain(a => a.PeriodNumber == 30);
        }
    }
}

