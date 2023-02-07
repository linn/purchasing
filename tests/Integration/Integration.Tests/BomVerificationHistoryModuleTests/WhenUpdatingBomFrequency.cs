namespace Linn.Purchasing.Integration.Tests.BomVerificationHistoryModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;
    using NSubstitute;
    using NUnit.Framework;

    public class WhenUpdatingBomFrequency : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var updatedResource = new BomFrequencyWeeksResource
            {
                PartNumber = "EXCEPT-THIS-PART",
                FreqWeeks = 5000
            };

            var request = new BomFrequencyWeeks
            {
                PartNumber = "EXCEPT-THIS-PART",
                FreqWeeks = 3
            };

            this.BomFrequencyRepository.FindById(request.PartNumber).Returns(request);

            this.Response = this.Client.PutAsJsonAsync(
                $"/purchasing/bom-verification/bom-frequency/{request.PartNumber}", updatedResource).Result;
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
        public void ShouldCallCommit()
        {
            this.TransactionManager.Received().Commit();
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<BomFrequencyWeeksResource>();
            resultResource.Should().NotBeNull();
            resultResource.PartNumber.Should().Be("EXCEPT-THIS-PART");
            resultResource.FreqWeeks.Should().Be(5000);
        }
    }
}
