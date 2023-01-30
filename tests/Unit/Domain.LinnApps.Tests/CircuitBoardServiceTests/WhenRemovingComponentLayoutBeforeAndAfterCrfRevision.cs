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

    public class WhenRemovingComponentLayoutBeforeAndAfterCrfRevision : ContextBase
    {
        private CircuitBoard result;

        private IEnumerable<BoardComponent> componentsToAdd;

        private IEnumerable<BoardComponent> componentsToRemove;

        [SetUp]
        public void SetUp()
        {
            this.ChangeRequest.RevisionCode = "L2R1";
            this.PcasChange.RevisionCode = "L2R1";
            this.Board.Layouts.Add(new BoardLayout
                                       {
                                           LayoutCode = "L2",
                                           LayoutSequence = 2,
                                           LayoutNumber = 2,
                                           Revisions = new List<BoardRevision>
                                                           {
                                                               new BoardRevision
                                                                   {
                                                                       BoardCode = this.BoardCode,
                                                                       LayoutCode = "L2",
                                                                       RevisionCode = "L2R1",
                                                                       RevisionNumber = 1,
                                                                       LayoutSequence = 2,
                                                                       VersionNumber = 1
                                                                   }
                                                           }
                                       });
            this.Board.Layouts.Add(new BoardLayout
                                       {
                                           LayoutCode = "L3",
                                           LayoutSequence = 3,
                                           LayoutNumber = 3,
                                           Revisions = new List<BoardRevision>
                                                           {
                                                               new BoardRevision
                                                                   {
                                                                       BoardCode = this.BoardCode,
                                                                       LayoutCode = "L3",
                                                                       RevisionCode = "L3R1",
                                                                       RevisionNumber = 1,
                                                                       LayoutSequence = 3,
                                                                       VersionNumber = 1
                                                                   }
                                                           }
                                       });

            this.componentsToAdd = null;
            this.componentsToRemove = new List<BoardComponent>
                                          {
                                              new BoardComponent
                                                  {
                                                      BoardCode = this.BoardCode,
                                                      BoardLine = 1,
                                                      CRef = "C001",
                                                      PartNumber = "CAP 123",
                                                      AssemblyTechnology = "SM",
                                                      ChangeState = "PROPOS",
                                                      FromLayoutVersion = 1,
                                                      FromRevisionVersion = 1,
                                                      ToLayoutVersion = null,
                                                      ToRevisionVersion = null,
                                                      AddChangeId = 123,
                                                      DeleteChangeId = null,
                                                      Quantity = 1
                                                  }
                                          };

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "CAP 123", AssemblyTechnology = "SM" });
            this.Board.Components.Should().HaveCount(1);

            this.result = this.Sut.UpdateComponents(
                this.BoardCode,
                this.PcasChange,
                this.ChangeRequestId,
                this.componentsToAdd,
                this.componentsToRemove);
        }

        [Test]
        public void ShouldMarkAsDeletedAndAddNewComponent()
        {
            this.result.Components.Should().HaveCount(3);
            var removed = this.result.Components.First(a => a.BoardLine == 1);
            removed.DeleteChangeId.Should().Be(this.ChangeId);
            var addedPrior = this.result.Components.First(a => a.BoardLine == 2);
            addedPrior.AddChangeId.Should().Be(this.ChangeId);
            addedPrior.FromLayoutVersion.Should().Be(1);
            addedPrior.ToLayoutVersion.Should().Be(1);
            addedPrior.FromRevisionVersion.Should().Be(1);
            addedPrior.ToRevisionVersion.Should().BeNull();
            var addedAfter = this.result.Components.First(a => a.BoardLine == 3);
            addedAfter.AddChangeId.Should().Be(this.ChangeId);
            addedAfter.FromLayoutVersion.Should().Be(3);
            addedAfter.ToLayoutVersion.Should().Be(null);
            addedAfter.FromRevisionVersion.Should().Be(1);
            addedAfter.ToRevisionVersion.Should().BeNull();
        }
    }
}
