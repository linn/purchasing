namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingComponents : ContextBase
    {
        private string boardCode;

        private CircuitBoard result;

        private PcasChange pcasChange;

        private int changeRequestId;

        private IEnumerable<BoardComponent> componentsToAdd;

        private IEnumerable<BoardComponent> componentsToRemove;

        private int changeId;

        private ChangeRequest changeRequest;

        private CircuitBoard board;

        [SetUp]
        public void SetUp()
        {
            this.boardCode = "123";
            this.changeId = 890;
            this.changeRequestId = 678;
            this.changeRequest = new ChangeRequest
                                     {
                                         DocumentNumber = this.changeRequestId,
                                         BoardCode = this.boardCode,
                                         RevisionCode = "L1R1",
                                         ChangeState = "PROPOS"
                                     };
            this.pcasChange = new PcasChange
                                  {
                                      BoardCode = this.boardCode,
                                      ChangeId = this.changeId,
                                      ChangeRequest = this.changeRequest,
                                      ChangeState = "PROPOS",
                                      DocumentNumber = this.changeRequestId,
                                      RevisionCode = "L1R1"
                                  };
            this.board = new CircuitBoard
                             {
                                 BoardCode = this.boardCode,
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
                                                                                       LayoutCode = "L1"
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
                                                              PartNumber = "CAP 123",
                                                              AssemblyTechnology = "SM",
                                                              ChangeState = "PROPOS",
                                                              FromLayoutVersion = 1,
                                                              FromRevisionVersion = 1,
                                                              ToLayoutVersion = null,
                                                              ToRevisionVersion = null,
                                                              AddChangeId = 8763458,
                                                              DeleteChangeId = null,
                                                              Quantity = 1
                                                          }
                                                  }
                             };
            this.componentsToAdd = new List<BoardComponent>
                                       {
                                           new BoardComponent
                                               {
                                                   BoardCode = this.boardCode,
                                                   BoardLine = 2,
                                                   CRef = "C001",
                                                   PartNumber = "CAP 123",
                                                   AssemblyTechnology = "SM",
                                                   ChangeState = "PROPOS",
                                                   FromLayoutVersion = 1,
                                                   FromRevisionVersion = 1,
                                                   ToLayoutVersion = null,
                                                   ToRevisionVersion = null,
                                                   AddChangeId = this.changeId,
                                                   DeleteChangeId = null,
                                                   Quantity = 1
                                               }
                                       };
            this.componentsToRemove = new List<BoardComponent>
                                       {
                                           new BoardComponent
                                               {
                                                   BoardCode = this.boardCode,
                                                   BoardLine = 1,
                                                   CRef = "C002",
                                                   PartNumber = "CAP 123",
                                                   AssemblyTechnology = "SM",
                                                   ChangeState = "PROPOS",
                                                   FromLayoutVersion = 1,
                                                   FromRevisionVersion = 1,
                                                   ToLayoutVersion = null,
                                                   ToRevisionVersion = null,
                                                   AddChangeId = 8763458,
                                                   DeleteChangeId = null,
                                                   Quantity = 1
                                               }
                                       };

            this.BoardRepository.FindById(this.boardCode).Returns(this.board);
            this.ChangeRequestRepository.FindById(this.changeRequestId).Returns(this.changeRequest);
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "CAP 123", AssemblyTechnology = "SM" });
            this.result = this.Sut.UpdateComponents(
                this.boardCode,
                this.pcasChange,
                this.changeRequestId,
                this.componentsToAdd,
                this.componentsToRemove);
        }

        [Test]
        public void ShouldLookUpBoard()
        {
            this.BoardRepository.Received().FindById(this.boardCode);
        }

        [Test]
        public void ShouldLookUpChangeRequest()
        {
            this.ChangeRequestRepository.Received().FindById(this.changeRequestId);
        }

        [Test]
        public void ShouldUpdateComponents()
        {
            this.result.Components.Should().HaveCount(2);
            var removed = this.result.Components.First(a => a.BoardLine == 1);
            removed.DeleteChangeId.Should().Be(this.changeId);
            var added = this.result.Components.First(a => a.BoardLine == 2);
            added.AddChangeId.Should().Be(this.changeId);
            added.AssemblyTechnology.Should().Be("SM");
        }
    }
}
