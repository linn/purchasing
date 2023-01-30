namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Collections.Generic;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingCircuitBoardComponentsWithDiscrepancies : ContextBase
    {
        private CircuitBoard board;

        private CircuitBoardComponentsUpdateResource updateResource;

        private int changeRequestId;

        [SetUp]
        public void SetUp()
        {
            this.changeRequestId = 123456;
            this.updateResource = new CircuitBoardComponentsUpdateResource
            {
                ChangeRequestId = this.changeRequestId,
                BoardCode = this.BoardCode,
                Description = "Desc",
                ClusterBoard = "Y",
                CoreBoard = "Y",
                IdBoard = "Y",
                SplitBom = "Y",
                Layouts = new List<BoardLayoutResource>
                              {
                                  new BoardLayoutResource
                                      {
                                          BoardCode = this.BoardCode,
                                          LayoutCode = "L1",
                                          LayoutSequence = 1,
                                          PcbNumber = "PCB",
                                          LayoutType = "L",
                                          LayoutNumber = 1,
                                          PcbPartNumber = "PCB PART",
                                          ChangeId = null,
                                          ChangeState = null,
                                          Revisions = new List<BoardRevisionResource>
                                                          {
                                                              new BoardRevisionResource
                                                                  {
                                                                      BoardCode = this.BoardCode,
                                                                      LayoutCode = "L1",
                                                                      RevisionCode = "L1R1",
                                                                      LayoutSequence = 1,
                                                                      VersionNumber = 1,
                                                                      RevisionType =
                                                                          new BoardRevisionTypeResource
                                                                              {
                                                                                  TypeCode = "PRODUCTION"
                                                                              },
                                                                      RevisionNumber = 1,
                                                                      SplitBom = "N",
                                                                      PcasPartNumber = "PCAS",
                                                                      PcsmPartNumber = "PCSM",
                                                                      PcbPartNumber = "PCB",
                                                                      AteTestCommissioned = null,
                                                                      ChangeId = null,
                                                                      ChangeState = null
                                                                  }
                                                          }
                                      },
                                  new BoardLayoutResource
                                      {
                                          BoardCode = this.BoardCode,
                                          LayoutCode = "L2",
                                          LayoutSequence = 2,
                                          PcbNumber = "PCB2",
                                          LayoutType = "L",
                                          LayoutNumber = 2,
                                          PcbPartNumber = "PCB PART2",
                                          ChangeId = null,
                                          ChangeState = null,
                                          Revisions = new List<BoardRevisionResource>
                                                          {
                                                              new BoardRevisionResource
                                                                  {
                                                                      BoardCode = this.BoardCode,
                                                                      LayoutCode = "L2",
                                                                      RevisionCode = "L2R1",
                                                                      LayoutSequence = 2,
                                                                      VersionNumber = 1,
                                                                      RevisionType =
                                                                          new BoardRevisionTypeResource
                                                                              {
                                                                                  TypeCode = "PRODUCTION"
                                                                              },
                                                                      RevisionNumber = 1,
                                                                      SplitBom = "N",
                                                                      PcasPartNumber = "PCAS2",
                                                                      PcsmPartNumber = "PCSM2",
                                                                      PcbPartNumber = "PCB2",
                                                                      AteTestCommissioned = null,
                                                                      ChangeId = null,
                                                                      ChangeState = null
                                                                  }
                                                          }
                                      }
                              },
                Components = new List<BoardComponentUpdateResource>
                                 {
                                     new BoardComponentUpdateResource
                                         {
                                             BoardCode = this.BoardCode,
                                             BoardLine = 1,
                                             CRef = "A001",
                                             PartNumber = "RES 123",
                                             AssemblyTechnology = "SM",
                                             ChangeState = "PROPOS",
                                             FromLayoutVersion = 1,
                                             FromRevisionVersion = 1,
                                             ToLayoutVersion = null,
                                             ToRevisionVersion = null,
                                             AddChangeId = 123,
                                             AddChangeDocumentType = null,
                                             AddChangeDocumentNumber = null,
                                             DeleteChangeId = null,
                                             DeleteChangeDocumentType = null,
                                             DeleteChangeDocumentNumber = null,
                                             Quantity = 1,
                                             Adding = true,
                                             Removing = null
                                         }
                                 }
            };
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
            this.CircuitBoardService.UpdateComponents(
                this.BoardCode,
                Arg.Any<PcasChange>(),
                this.changeRequestId,
                Arg.Any<IEnumerable<BoardComponent>>(),
                Arg.Any<IEnumerable<BoardComponent>>()).Returns(this.board);
            this.PcasPack
                .DiscrepanciesOnChange(this.BoardCode, this.updateResource.ChangeRequestRevisionCode, Arg.Any<int>())
                .Returns("This is a list of discrepancies");
            this.Response = this.Client.PutAsJsonAsync($"/purchasing/boms/board-components/{this.BoardCode}", this.updateResource)
                .Result;
        }

        [Test]
        public void ShouldReturnResourceWithDiscrepancies()
        {
            var resultResource = this.Response.DeserializeBody<CircuitBoardResource>();
            resultResource.Discrepancies.Should().Be("This is a list of discrepancies");
        }
    }
}
