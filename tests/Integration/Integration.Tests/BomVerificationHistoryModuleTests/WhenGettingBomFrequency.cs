namespace Linn.Purchasing.Integration.Tests.BomVerificationHistoryModuleTests
{
    using System.Net;

    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenGettingBomFrequency : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var request = new BomFrequencyWeeks
            {
                PartNumber = "A-PART-LIKE-NO-OTHER",
                FreqWeeks = 222
            };

            this.BomFrequencyRepository.FindById(request.PartNumber).Returns(request);

            this.Response = this.Client.Get(
                $"/purchasing/bom-verification/bom-frequency/{request.PartNumber}",
                with =>
                {
                    with.Accept("application/json");
                }).Result;
        }

        [Test]
        public void ShouldReturnCreated()
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
            var resultResource = this.Response.DeserializeBody<BomFrequencyWeeksResource>();
            resultResource.Should().NotBeNull();
            resultResource.PartNumber.Should().Be("A-PART-LIKE-NO-OTHER");
            resultResource.FreqWeeks.Should().Be(222);
        }
    }
}
