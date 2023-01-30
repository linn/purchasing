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

    public class WhenRemovingComponentToRevisionAfterCrfRevision : ContextBase
    {
        private CircuitBoard result;

        private IEnumerable<BoardComponent> componentsToAdd;

        private IEnumerable<BoardComponent> componentsToRemove;

        [SetUp]
        public void SetUp()
        {
            this.ChangeRequest.RevisionCode = "L1R2";
            this.PcasChange.RevisionCode = "L1R2";

            this.Board.Layouts.First().Revisions.Add(new BoardRevision
                                                         {
                                                             BoardCode = this.BoardCode,
                                                             LayoutCode = "L1",
                                                             RevisionCode = "L1R2",
                                                             RevisionNumber = 2,
                                                             LayoutSequence = 1,
                                                             VersionNumber = 2
                                                         });
            this.Board.Layouts.First().Revisions.Add(new BoardRevision
                                                         {
                                                             BoardCode = this.BoardCode,
                                                             LayoutCode = "L1",
                                                             RevisionCode = "L1R3",
                                                             RevisionNumber = 3,
                                                             LayoutSequence = 1,
                                                             VersionNumber = 3
                                                         });
            this.Board.Components.First().FromLayoutVersion = 1;
            this.Board.Components.First().FromRevisionVersion = 2;

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
                                                      FromRevisionVersion = 2,
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
            this.result.Components.Should().HaveCount(2);
            var removed = this.result.Components.First(a => a.BoardLine == 1);
            removed.DeleteChangeId.Should().Be(this.ChangeId);
            var added = this.result.Components.First(a => a.BoardLine == 2);
            added.AddChangeId.Should().Be(this.ChangeId);
            added.FromLayoutVersion.Should().Be(1);
            added.ToLayoutVersion.Should().BeNull();
            added.FromRevisionVersion.Should().Be(3);
            added.ToRevisionVersion.Should().BeNull();
        }
    }
}
