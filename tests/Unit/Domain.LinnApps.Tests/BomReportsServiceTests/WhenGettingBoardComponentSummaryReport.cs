﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.BomReportsServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBoardComponentSummaryReport : ContextBase
    {
        private ResultsModel results;

        private string revisionCode;

        private string boardCode;

        private CircuitBoard board;

        [SetUp]
        public void SetUp()
        {
            this.boardCode = "1";
            this.revisionCode = "L1R1";
            this.board = new CircuitBoard
                              {
                                  BoardCode = this.boardCode,
                                  Description = "Standard Board",
                                  ChangeId = 123,
                                  ChangeState = "Yip",
                                  SplitBom = "Y",
                                  DefaultPcbNumber = "P231/3",
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
                                                            BoardCode = this.boardCode,
                                                            LayoutCode = "L1",
                                                            LayoutNumber = 1,
                                                            LayoutSequence = 1,
                                                            LayoutType = "PRODUCTION",
                                                            Revisions = new List<BoardRevision>
                                                                            {
                                                                                new BoardRevision
                                                                                    {
                                                                                        BoardCode = this.boardCode,
                                                                                        RevisionCode = "L1R1",
                                                                                        RevisionNumber = 1,
                                                                                        VersionNumber = 1,
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
                                                               BoardCode = this.boardCode,
                                                               BoardLine = 1,
                                                               CRef = "C002",
                                                               PartNumber = "CAP 500",
                                                               AssemblyTechnology = "SM",
                                                               ChangeState = "PROPOS",
                                                               FromLayoutVersion = 1,
                                                               FromRevisionVersion = 1,
                                                               ToLayoutVersion = 1,
                                                               ToRevisionVersion = 1,
                                                               AddChangeId = 98765432,
                                                               DeleteChangeId = null,
                                                               Quantity = 1,
                                                               Part = new Part
                                                                          {
                                                                              PartNumber = "TEST 987",
                                                                              Description = "A test part that we are describing"
                                                                          }
                                                           },
                                                       new BoardComponent
                                                           {
                                                               BoardCode = this.boardCode,
                                                               BoardLine = 3,
                                                               CRef = "C003",
                                                               PartNumber = "CAP 900",
                                                               AssemblyTechnology = "SM",
                                                               ChangeState = "PROPOS",
                                                               FromLayoutVersion = 1,
                                                               FromRevisionVersion = 1,
                                                               ToLayoutVersion = 1,
                                                               ToRevisionVersion = 1,
                                                               AddChangeId = 1234567,
                                                               DeleteChangeId = null,
                                                               Quantity = 1,
                                                               Part = new Part
                                                                          {
                                                                              PartNumber = "TEST 123",
                                                                              Description = "A test part that we are describing"
                                                                          }
                                                           }
                                                   }
                              };

            this.CircuitBoardRepository.FindById(this.boardCode).Returns(this.board);
            this.results = this.Sut.GetBoardComponentSummaryReport(
                this.boardCode,
                this.revisionCode);
        }

        [Test]
        public void ShouldCallRepositoryOnce()
        {
            this.CircuitBoardRepository.Received(1).FindById(Arg.Any<string>());
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.results.Rows.Count().Should().Be(2);
            this.results.GetGridTextValue(0, 0).Should().Be("C002");
            this.results.GetGridTextValue(0, 1).Should().Be("Standard Board");
            this.results.GetGridTextValue(0, 2).Should().Be("CAP 500");
            this.results.GetGridTextValue(0, 3).Should().Be("A test part that we are describing");
            this.results.GetGridTextValue(0, 4).Should().BeNull();
            this.results.GetGridTextValue(0, 5).Should().Be("SM");
            this.results.GetGridTextValue(1, 0).Should().Be("C003");
            this.results.GetGridTextValue(1, 1).Should().Be("Standard Board");
            this.results.GetGridTextValue(1, 2).Should().Be("CAP 900");
            this.results.GetGridTextValue(1, 3).Should().Be("A test part that we are describing");
            this.results.GetGridTextValue(1, 4).Should().BeNull();
            this.results.GetGridTextValue(1, 5).Should().Be("SM");
        }
    }
}
