namespace Linn.Purchasing.Integration.Tests.BomVerificationHistoryModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenSearchingBomVerificationHistoryEntries : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var bomVerifificationHistoryEntry = new BomVerificationHistory
            {
                TRef = 123,
                DocumentNumber = 654321,
                DocumentType = "Test",
                PartNumber = "RES 100",
                Remarks = "D.Skinmer",
                VerifiedBy = 33086
            };
            
            this.BomVerificationHistoryRepository.FilterBy(Arg.Any<Expression<Func<BomVerificationHistory, bool>>>())
                .Returns(new List<BomVerificationHistory> { bomVerifificationHistoryEntry }.AsQueryable());
            this.Response = this.Client.Get(
                "/purchasing/bom-verification?searchTerm=something",
                with => { with.Accept("application/json"); }).Result;
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
        public void ShouldCallRepository()
        {
            this.BomVerificationHistoryRepository.Received().FilterBy(Arg.Any<Expression<Func<BomVerificationHistory, bool>>>());
        }

        [Test]
        public void ShouldBuildResource()
        {
            var result = this.Response.DeserializeBody<IEnumerable<BomVerificationHistoryResource>>().ToList();
            result.Should().HaveCount(1);
            result.First().TRef.Should().Be(123);
        }
    }
}
