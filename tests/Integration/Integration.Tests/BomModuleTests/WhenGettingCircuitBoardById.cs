﻿namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingCircuitBoardById : ContextBase
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
            
            this.CircuitBoardRepository.FindById(this.boardCode).Returns(board);

            this.Response = this.Client.Get(
                $"/purchasing/boms/boards/{this.boardCode}",
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
            this.CircuitBoardRepository.Received().FindById(this.boardCode);
        }

        [Test]
        public void ShouldBuildResource()
        {
            var result = this.Response.DeserializeBody<CircuitBoardResource>();
            result.Should().NotBeNull();
            result.BoardCode.Should().Be(this.boardCode);
            result.Description.Should().Be("board");
            result.Links.First(a => a.Rel == "self").Href.Should().Be($"/purchasing/boms/boards/{this.boardCode}");
        }
    }
}
