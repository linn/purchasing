namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingCircuitBoard : ContextBase
    {
        private CircuitBoardResource resource;

        private string boardCode;

        private CircuitBoard board;

        [SetUp]
        public void SetUp()
        {
            this.boardCode = "1234";
            this.resource = new CircuitBoardResource
                                {
                                    BoardCode = this.boardCode,
                                    Description = "Desc",
                                    ClusterBoard = "Y",
                                    CoreBoard = "Y",
                                    IdBoard = "Y",
                                    SplitBom = "Y"
                                };
            this.board = new CircuitBoard { BoardCode = this.boardCode };
            this.CircuitBoardRepository.FindById(this.boardCode).Returns(this.board);

            this.Response = this.Client.PutAsJsonAsync(
                $"/purchasing/boms/boards/{this.boardCode}",
                this.resource).Result;
        }

        [Test]
        public void ShouldReturnSuccess()
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
            var resultResource = this.Response.DeserializeBody<CircuitBoardResource>();
            resultResource.Should().NotBeNull();
            resultResource.BoardCode.Should().Be(this.boardCode);
            resultResource.Description.Should().Be(this.resource.Description);
            resultResource.ClusterBoard.Should().Be(this.resource.ClusterBoard);
            resultResource.CoreBoard.Should().Be(this.resource.CoreBoard);
            resultResource.IdBoard.Should().Be(this.resource.IdBoard);
        }
    }
}
