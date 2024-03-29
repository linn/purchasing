﻿namespace Linn.Purchasing.Integration.Tests.BomVerificationHistoryModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;
    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBomVerificationHistoryEntryById : ContextBase
    {
        private BomVerificationHistoryResource result;

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
            this.result = this.Response.DeserializeBody<BomVerificationHistoryResource>();
            this.result.TRef.Should().Be(123);
            this.result.DocumentNumber.Should().Be(654321);
            this.result.DocumentType.Should().Be("Test");
            this.result.Remarks.Should().Be("D.Skinmer");
            this.result.VerifiedBy.Should().Be(33086);
        }

        [Test]
        public void ShouldReturnCorrectHeaderLinks()
        {
            result = this.Response.DeserializeBody<BomVerificationHistoryResource>();
            result.Links.First(a => a.Rel == "self").Href.Should().Be($"/purchasing/bom-verification/123");
        }
    }
}
