namespace Linn.Purchasing.Integration.Tests.AddressModule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingCountries : ContextBase
    {
        private IEnumerable<Country> data;

        [SetUp]
        public void SetUp()
        {
            this.data = new List<Country>
                            {
                                new Country
                                    {
                                       CountryCode = "GB",
                                       Name = "BRITAIN"
                                    },
                                new Country
                                    {
                                        CountryCode = "US",
                                        Name = "MERICA"
                                    },
                            };
            this.CountryRepository.FilterBy(Arg.Any<Expression<Func<Country, bool>>>())
                .Returns(this.data.AsQueryable());

            this.Response = this.Client.Get(
                "/purchasing/countries?searchTerm=sam",
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
            this.Response.Content.Headers.ContentType.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resources = this.Response.DeserializeBody<IEnumerable<CountryResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);
        }

        [Test]
        public void ShouldBuildResource()
        {
            var resource = this.Response.DeserializeBody<IEnumerable<CountryResource>>().ToArray().First();
            resource.CountryCode.Should().Be("GB");
            resource.Name.Should().Be("BRITAIN");
        }
    }
}
