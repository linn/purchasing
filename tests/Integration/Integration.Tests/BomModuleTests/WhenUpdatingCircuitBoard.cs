namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
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
        private CircuitBoard board;

        [SetUp]
        public void SetUp()
        {
            this.board = new CircuitBoard
                             {
                                 BoardCode = this.BoardCode,
                                 Layouts = new List<BoardLayout>
                                               {
                                                   new BoardLayout
                                                       {
                                                           BoardCode = this.BoardCode,
                                                           LayoutCode = "L1",
                                                           LayoutSequence = 1,
                                                           PcbNumber = "OLD",
                                                           LayoutType = "L",
                                                           LayoutNumber = 1,
                                                           PcbPartNumber = "OLD",
                                                           ChangeId = null,
                                                           ChangeState = null,
                                                           Revisions = new List<BoardRevision>
                                                                           {
                                                                               new BoardRevision
                                                                                   {
                                                                                       BoardCode = this.BoardCode,
                                                                                       LayoutCode = "L1",
                                                                                       RevisionCode = "L1R1",
                                                                                       LayoutSequence = 1,
                                                                                       VersionNumber = 1,
                                                                                       RevisionType =
                                                                                           new BoardRevisionType
                                                                                               {
                                                                                                   TypeCode = "PRODUCTION"
                                                                                               },
                                                                                       RevisionNumber = 1,
                                                                                       SplitBom = "N",
                                                                                       PcasPartNumber = "OLD",
                                                                                       PcsmPartNumber = "OLD",
                                                                                       PcbPartNumber = "OLD",
                                                                                       AteTestCommissioned = null,
                                                                                       ChangeId = null,
                                                                                       ChangeState = null
                                                                                   }
                                                                           }
                                                       }
                                               }
                             };
            this.CircuitBoardRepository.FindById(this.BoardCode).Returns(this.board);

            this.Response = this.Client.PutAsJsonAsync($"/purchasing/boms/boards/{this.BoardCode}", this.Resource)
                .Result;
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
            resultResource.BoardCode.Should().Be(this.BoardCode);
            resultResource.Description.Should().Be(this.Resource.Description);
            resultResource.ClusterBoard.Should().Be(this.Resource.ClusterBoard);
            resultResource.CoreBoard.Should().Be(this.Resource.CoreBoard);
            resultResource.IdBoard.Should().Be(this.Resource.IdBoard);
            resultResource.Layouts.Should().HaveCount(2);
            var layout = resultResource.Layouts.First(a => a.LayoutCode == "L1");
            layout.LayoutCode.Should().Be("L1");
            layout.PcbPartNumber.Should().Be("PCB PART");
            layout.Revisions.Should().HaveCount(1);
            var revision = layout.Revisions.First();
            revision.PcasPartNumber.Should().Be("PCAS");
            revision.RevisionCode.Should().Be("L1R1");
            var layout2 = resultResource.Layouts.First(a => a.LayoutCode == "L2");
            layout2.LayoutCode.Should().Be("L2");
            layout2.PcbPartNumber.Should().Be("PCB PART2");
            layout2.Revisions.Should().HaveCount(1);
            var revision2 = layout2.Revisions.First();
            revision2.PcasPartNumber.Should().Be("PCAS2");
            revision2.RevisionCode.Should().Be("L2R1");
        }
    }
}
