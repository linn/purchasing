namespace Linn.Purchasing.Integration.Tests.AddressModule
{
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreating : ContextBase
    {
        private AddressResource addressResource;

        [SetUp]
        public void SetUp()
        {
            this.addressResource = new AddressResource
                                       {
                                           AddressId = 1,
                                           Addressee = "SAM",
                                           Addressee2 = "UEL",
                                           Line1 = "A",
                                           Line2 = "B",
                                           Line3 = "C",
                                           Line4 = "D",
                                           PostCode = "G21 4AX",
                                           CountryCode = "GB",
                                           CountryName = "GREAT BRITAIN"
                                       };
            this.CountryRepository.FindById("GB").Returns(new Country { CountryCode = "GB", Name = "GREAT BRITAIN" });
            this.DatabaseService.GetNextVal("ADDR_SEQ").Returns(1);
            this.Response = this.Client.Post(
                "/purchasing/addresses",
                this.addressResource,
                with =>
                    {
                        with.Accept("application/json");
                    }).Result;
        }

        [Test]
        public void ShouldReturnCreated()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
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
            var resource = this.Response.DeserializeBody<AddressResource>();
            resource.Should().NotBeNull();
        }

        [Test]
        public void ShouldBuildResource()
        {
            var resource = this.Response.DeserializeBody<AddressResource>();
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
