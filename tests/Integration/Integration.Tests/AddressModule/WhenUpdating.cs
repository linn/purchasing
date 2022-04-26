namespace Linn.Purchasing.Integration.Tests.AddressModule
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private AddressResource resource;

        private int id;

        [SetUp]
        public void SetUp()
        {
            this.id = 123;

            this.resource = new AddressResource
                                {
                                    AddressId = 123,
                                    Addressee = "SAM",
                                    Addressee2 = "UEL",
                                    Line1 = "A",
                                    Line2 = "B",
                                    Line3 = "C",
                                    Line4 = "D",
                                    PostCode = "G21 4AX",
                                    CountryCode = "NC",
                                };

            this.CountryRepository.FindById("NC").Returns(new Country { Name = "NEW COUNTRY", CountryCode = "NC" });
            this.AddressRepository.FindById(123).Returns(new Address
                                                             {
                                                                 AddressId = 1,
                                                                 Addressee = "SAM",
                                                                 Addressee2 = "UEL",
                                                                 Line1 = "A",
                                                                 Line2 = "B",
                                                                 Line3 = "C",
                                                                 Line4 = "D",
                                                                 PostCode = "G21 4AX",
                                                                 Country = new Country { CountryCode = "NC", Name = "A NEW COUNTRY" }
                                                             });

            this.Response = this.Client.PutAsJsonAsync(
                $"/purchasing/addresses/{this.id}",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
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
        public void ShouldReturnUpdated()
        {
            var resultResource = this.Response.DeserializeBody<AddressResource>();
            resultResource.Should().NotBeNull();

            resultResource.CountryCode.Should().Be("NC");
        }
    }
}
