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

    public class WhenSearching : ContextBase
    {
        private IEnumerable<Address> data;

        [SetUp]
        public void SetUp()
        {
            this.data = new List<Address>
                            {
                                new Address 
                                    { 
                                        AddressId = 1,
                                        Addressee = "SAM",
                                        Addressee2 = "UEL",
                                        Line1 = "A",
                                        Line2 = "B",
                                        Line3 = "C",
                                        Line4 = "D",
                                        PostCode = "G21 4AX",
                                        Country = new Country { CountryCode = "GB", Name = "GREAT BRITAIN" }
                                    },
                                new Address 
                                    { 
                                        AddressId = 2, 
                                        Country = new Country { CountryCode = "GB" }
                                    },
                            };
            this.AddressRepository.FilterBy(Arg.Any<Expression<Func<Address, bool>>>())
                .Returns(this.data.AsQueryable());
            
            this.Response = this.Client.Get(
                "/purchasing/addresses?searchTerm=sam",
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
            var resources = this.Response.DeserializeBody<IEnumerable<AddressResource>>()?.ToArray();
            resources.Should().NotBeNull();
            resources.Should().HaveCount(2);
        }

        [Test]
        public void ShouldBuildResource()
        {
            var resource = this.Response.DeserializeBody<IEnumerable<AddressResource>>().ToArray().First();
            resource.AddressId.Should().Be(1);
            resource.Addressee.Should().Be("SAM");
            resource.Addressee2.Should().Be("UEL");
            resource.Line1.Should().Be("A");
            resource.Line2.Should().Be("B");
            resource.Line3.Should().Be("C");
            resource.Line4.Should().Be("D");
            resource.PostCode.Should().Be("G21 4AX");
            resource.CountryCode.Should().Be("GB");
            resource.CountryName.Should().Be("GREAT BRITAIN");
        }
    }
}
