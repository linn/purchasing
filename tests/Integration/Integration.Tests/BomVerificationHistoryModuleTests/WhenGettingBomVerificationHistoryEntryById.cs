namespace Linn.Purchasing.Integration.Tests.BomVerificationHistoryModuleTests
{
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;
    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBomVerificationHistoryEntryById : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var bomVerificationHistoryEntry = new BomVerificationHistory
            {
                TRef = 123,
                DocumentNumber = 654321,
                DocumentType = "Test",
                PartNumber = "RES 100",
                Remarks = "D.Skinmer",
                VerifiedBy = 33086
            };

            this.BomVerificationHistoryRepository.FindById(123).Returns(bomVerificationHistoryEntry);

            this.Response = this.Client.Get(
                "/purchasing/bom-verification/123",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCallRepository()
        {
            this.BomVerificationHistoryRepository.Received().FindById(123);
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
            var result = this.Response.DeserializeBody<BomVerificationHistoryResource>();
            result.TRef.Should().Be(123);
            result.DocumentNumber.Should().Be(654321);
            result.DocumentType.Should().Be("Test");
            result.Remarks.Should().Be("D.Skinmer");
            result.VerifiedBy.Should().Be(33086);
        }
    }
}
