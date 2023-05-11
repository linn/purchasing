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

    public class WhenRemovingComponentStartingRevisionBeforeCrfRevisionAndAdding : ContextBase
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
            this.componentsToAdd = new List<BoardComponent>
                                       {
                                           new BoardComponent
                                               {
                                                   BoardCode = this.BoardCode,
                                                   BoardLine = 2,
                                                   CRef = "C001",
                                                   PartNumber = "CAP 123",
                                                   AssemblyTechnology = "SM",
                                                   ChangeState = "PROPOS",
                                                   FromLayoutVersion = 1,
                                                   FromRevisionVersion = 1,
                                                   ToLayoutVersion = null,
                                                   ToRevisionVersion = null,
                                                   AddChangeId = this.ChangeId,
                                                   DeleteChangeId = null,
                                                   Quantity = 1
                                               }
                                       };
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
            var addedForRemove = this.result.Components.First(a => a.BoardLine == 3);
            addedForRemove.AddChangeId.Should().Be(this.ChangeId);
            addedForRemove.FromLayoutVersion.Should().Be(1);
            addedForRemove.ToLayoutVersion.Should().Be(1);
            addedForRemove.FromRevisionVersion.Should().Be(1);
            addedForRemove.ToRevisionVersion.Should().Be(1);
            var addedByRequest = this.result.Components.First(a => a.BoardLine == 2);
            addedByRequest.ToLayoutVersion.Should().BeNull();
        }
    }
}
