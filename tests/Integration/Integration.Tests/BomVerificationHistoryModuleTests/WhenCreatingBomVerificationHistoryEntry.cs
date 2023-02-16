namespace Linn.Purchasing.Integration.Tests.BomVerificationHistoryModuleTests
{
    using System;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;

    using NUnit.Framework;

    public class WhenCreatingBomVerificationHistoryEntry : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var request = new BomVerificationHistoryResource
            {
                TRef = 0,
                DocumentNumber = 555666,
                DocumentType = "Test",
                PartNumber = "CAP 500",
                Remarks = "B.Slime",
                VerifiedBy = 33084,
                DateVerified = new DateTime(2023, 1, 19, 6, 0, 0).ToString("dd-MMM-yyyy")
            };

            this.Response = this.Client.PostAsJsonAsync(
                $"/purchasing/bom-verification/create",
                request).Result;
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
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<BomVerificationHistoryResource>();
            resultResource.Should().NotBeNull();
            resultResource.TRef.Should().Be(0);
            resultResource.DocumentNumber.Should().Be(555666);
            resultResource.DocumentType.Should().Be("Test");
            resultResource.PartNumber.Should().Be("CAP 500");
            resultResource.Remarks.Should().Be("B.Slime");
            resultResource.VerifiedBy.Should().Be(33084);
            resultResource.DateVerified.Should().Be(DateTime.Now.ToString("dd-MMM-yyyy"));
        }
    }
}
