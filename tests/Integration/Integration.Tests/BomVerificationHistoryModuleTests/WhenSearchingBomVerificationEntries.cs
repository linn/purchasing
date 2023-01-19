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

    public class WhenSearchingBomVerificationEntries : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.BomVerificationHistoryRepository.FilterBy(Arg.Any<Expression<Func<BomVerificationHistory, bool>>>())
                .Returns(new List<BomVerificationHistory>
                             {
                                 new BomVerificationHistory
                                     {
                                         TRef = 312,
                                         DocumentNumber = 123456,
                                         DocumentType = "Test",
                                         PartNumber = "BIF 800",
                                         Remarks = "D.Fyne",
                                         VerifiedBy = 33085
                                     }
                             }.AsQueryable());

            this.Response = this.Client.Get(
                "/purchasing/bom-verification/?searchTerm=123456",
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
            result.First().TRef.Should().Be(312);
            result.First().DocumentNumber.Should().Be(123456);
            result.First().DocumentType.Should().Be("Test");
            result.First().PartNumber.Should().Be("BIF 800");
            result.First().Remarks.Should().Be("D.Fyne");
            result.First().VerifiedBy.Should().Be(33085);
        }
    }
}
