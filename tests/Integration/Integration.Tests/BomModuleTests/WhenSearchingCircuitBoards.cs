namespace Linn.Purchasing.Integration.Tests.BomModuleTests
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

    public class WhenSearchingCircuitBoards : ContextBase
    {
        private string boardCode;

        [SetUp]
        public void SetUp()
        {
            this.boardCode = "180";
            var board = new CircuitBoard
                            {
                                BoardCode = this.boardCode,
                                Description = "board",
                                ChangeId = 1,
                                ChangeState = "some state",
                                SplitBom = "N",
                                DefaultPcbNumber = "180",
                                VariantOfBoardCode = "B",
                                LoadDirectory = "o",
                                BoardsPerSheet = 12,
                                CoreBoard = "N",
                                ClusterBoard = "Y",
                                IdBoard = "N"
                            };
            
            this.CircuitBoardRepository.FilterBy(Arg.Any<Expression<Func<CircuitBoard, bool>>>())
                .Returns(new List<CircuitBoard> { board }.AsQueryable());
            this.Response = this.Client.Get(
                "/purchasing/boms/boards?searchTerm=something",
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
            this.CircuitBoardRepository.Received().FilterBy(Arg.Any<Expression<Func<CircuitBoard, bool>>>());
        }

        [Test]
        public void ShouldBuildResource()
        {
            var result = this.Response.DeserializeBody<IEnumerable<CircuitBoardResource>>().ToList();
            result.Should().HaveCount(1);
            result.First().BoardCode.Should().Be(this.boardCode);
        }
    }
}
