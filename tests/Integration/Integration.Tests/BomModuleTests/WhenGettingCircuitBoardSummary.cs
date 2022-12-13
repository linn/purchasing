namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingCircuitBoardSummary : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var board = new BoardComponentSummary
                            {
                                BoardCode = "123",
                                RevisionCode = "L1R1",
                                Cref = "C006",
                                PartNumber = "CAP 543",
                                AssemblyTechnology = "SM",
                                Quantity = 1,
                                BoardLine = 0,
                                ChangeState = "LIVE",
                                AddChangeId = 0,
                                DeleteChangeId = null,
                                FromLayoutVersion = 0,
                                FromRevisionVersion = 0,
                                ToLayoutVersion = null,
                                ToRevisionVersion = null,
                                LayoutSequence = 0,
                                VersionNumber = 0,
                                BomPartNumber = "PCAS 123/L1R1",
                                PcasPartNumber = null,
                                PcsmPartNumber = null,
                                PcbPartNumber = null
                            };
            
            this.BoardComponentSummaryRepository.FilterBy(Arg.Any<Expression<Func<BoardComponentSummary, bool>>>())
                .Returns(new List<BoardComponentSummary> { board }.AsQueryable());

            this.Response = this.Client.Get(
                "/purchasing/boms/boards-summary?boardCode=123",
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
            this.BoardComponentSummaryRepository.Received().FilterBy(Arg.Any<Expression<Func<BoardComponentSummary, bool>>>());
        }

        [Test]
        public void ShouldBuildResource()
        {
            var result = this.Response.DeserializeBody<IEnumerable<BoardComponentSummaryResource>>().ToList();
            result.Should().HaveCount(1);
            result.First().BoardCode.Should().Be("123");
        }
    }
}
