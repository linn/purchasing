namespace Linn.Purchasing.Domain.LinnApps.Tests.BomReportsServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBoardDifference : ContextBase
    {
        private ResultsModel results;

        private string revisionCode2;

        private string boardCode1;

        private string revisionCode1;

        private string boardCode2;

        private CircuitBoard board1;

        [SetUp]
        public void SetUp()
        {
            this.boardCode1 = "1";
            this.revisionCode1 = "L1R1";
            this.boardCode2 = "1";
            this.revisionCode2 = "L1R2";
            this.board1 = new CircuitBoard
            {
                BoardCode = this.boardCode1,
                Description = null,
                ChangeId = null,
                ChangeState = null,
                SplitBom = null,
                DefaultPcbNumber = null,
                VariantOfBoardCode = null,
                LoadDirectory = null,
                BoardsPerSheet = null,
                CoreBoard = null,
                ClusterBoard = null,
                IdBoard = null,
                Layouts = new List<BoardLayout>
                              {
                                  new BoardLayout
                                      {
                                          BoardCode = this.boardCode1,
                                          LayoutCode = "L1",
                                          LayoutNumber = 1,
                                          LayoutSequence = 1,
                                          LayoutType = "PRODUCTION",
                                          Revisions = new List<BoardRevision>
                                                          {
                                                              new BoardRevision
                                                                  {
                                                                      BoardCode = this.boardCode1,
                                                                      RevisionCode = "L1R1",
                                                                      RevisionNumber = 1,
                                                                      VersionNumber = 1,
                                                                      LayoutCode = "L1",
                                                                      LayoutSequence = 1
                                                                  },
                                                              new BoardRevision
                                                                  {
                                                                      BoardCode = this.boardCode1,
                                                                      RevisionCode = "L1R2",
                                                                      RevisionNumber = 2,
                                                                      VersionNumber = 2,
                                                                      LayoutCode = "L1",
                                                                      LayoutSequence = 1
                                                                  }
                                                          }
                                      }
                              },
                Components = new List<BoardComponent>
                                 {
                                     new BoardComponent
                                         {
                                             BoardCode = this.boardCode1,
                                             BoardLine = 1,
                                             CRef = "C002",
                                             PartNumber = "CAP 123",
                                             AssemblyTechnology = "SM",
                                             ChangeState = "PROPOS",
                                             FromLayoutVersion = 1,
                                             FromRevisionVersion = 1,
                                             ToLayoutVersion = 1,
                                             ToRevisionVersion = 1,
                                             AddChangeId = 8763458,
                                             DeleteChangeId = null,
                                             Quantity = 1
                                         },
                                     new BoardComponent
                                         {
                                             BoardCode = this.boardCode1,
                                             BoardLine = 2,
                                             CRef = "C002",
                                             PartNumber = "CAP 124",
                                             AssemblyTechnology = "SM",
                                             ChangeState = "PROPOS",
                                             FromLayoutVersion = 1,
                                             FromRevisionVersion = 2,
                                             ToLayoutVersion = null,
                                             ToRevisionVersion = null,
                                             AddChangeId = 8763458,
                                             DeleteChangeId = null,
                                             Quantity = 1
                                         },
                                     new BoardComponent
                                         {
                                             BoardCode = this.boardCode1,
                                             BoardLine = 3,
                                             CRef = "C003",
                                             PartNumber = "CAP 125",
                                             AssemblyTechnology = "SM",
                                             ChangeState = "PROPOS",
                                             FromLayoutVersion = 1,
                                             FromRevisionVersion = 1,
                                             ToLayoutVersion = 1,
                                             ToRevisionVersion = 1,
                                             AddChangeId = 8763458,
                                             DeleteChangeId = null,
                                             Quantity = 1
                                         },
                                     new BoardComponent
                                         {
                                             BoardCode = this.boardCode1,
                                             BoardLine = 4,
                                             CRef = "C004",
                                             PartNumber = "CAP 126",
                                             AssemblyTechnology = "SM",
                                             ChangeState = "PROPOS",
                                             FromLayoutVersion = 1,
                                             FromRevisionVersion = 2,
                                             ToLayoutVersion = null,
                                             ToRevisionVersion = null,
                                             AddChangeId = 8763458,
                                             DeleteChangeId = null,
                                             Quantity = 1
                                         }
                                 }
            };

            this.CircuitBoardRepository.FindById(this.boardCode1).Returns(this.board1);

            this.results = this.Sut.GetBoardDifferenceReport(
                this.boardCode1,
                this.revisionCode1,
                this.boardCode2,
                this.revisionCode2);
        }

        [Test]
        public void ShouldCallRepositoryOnce()
        {
            this.CircuitBoardRepository.Received(1).FindById(Arg.Any<string>());
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.results.Rows.Count().Should().Be(3);
            this.results.GetGridTextValue(0, 0).Should().Be("CAP 123");
            this.results.GetGridTextValue(0, 1).Should().Be("SM");
            this.results.GetGridTextValue(0, 2).Should().Be("1.0");
            this.results.GetGridTextValue(0, 3).Should().Be("CAP 124");
            this.results.GetGridTextValue(0, 4).Should().Be("SM");
            this.results.GetGridTextValue(0, 5).Should().Be("1.0");
            this.results.GetGridTextValue(1, 0).Should().Be("CAP 125");
            this.results.GetGridTextValue(1, 1).Should().Be("SM");
            this.results.GetGridTextValue(1, 2).Should().Be("1.0");
            this.results.GetGridTextValue(1, 3).Should().BeNull();
            this.results.GetGridTextValue(1, 4).Should().BeNull();
            this.results.GetGridTextValue(1, 5).Should().BeNull();
            this.results.GetGridTextValue(2, 0).Should().BeNull();
            this.results.GetGridTextValue(2, 1).Should().BeNull();
            this.results.GetGridTextValue(2, 2).Should().BeNull();
            this.results.GetGridTextValue(2, 3).Should().Be("CAP 126");
            this.results.GetGridTextValue(2, 4).Should().Be("SM");
            this.results.GetGridTextValue(2, 5).Should().Be("1.0");
        }
    }
}
