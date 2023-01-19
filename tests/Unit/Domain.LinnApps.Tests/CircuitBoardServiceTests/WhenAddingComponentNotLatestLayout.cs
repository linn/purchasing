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

    public class WhenAddingComponentNotLatestLayout : ContextBase
    {
        private CircuitBoard result;

        private PcasChange pcasChange;

        private IEnumerable<BoardComponent> componentsToAdd;

        private IEnumerable<BoardComponent> componentsToRemove;

        private int changeId;

        [SetUp]
        public void SetUp()
        {
            this.changeId = 890;

            this.pcasChange = new PcasChange
                                  {
                                      BoardCode = this.BoardCode,
                                      ChangeId = this.changeId,
                                      ChangeRequest = this.ChangeRequest,
                                      ChangeState = "PROPOS",
                                      DocumentNumber = this.ChangeRequestId,
                                      RevisionCode = "L1R1"
                                  };
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
                                                   AddChangeId = this.changeId,
                                                   DeleteChangeId = null,
                                                   Quantity = 1
                                               }
                                       };
            this.componentsToRemove = null;
            this.Board.Layouts.Add(new BoardLayout
                                       {
                                           BoardCode = this.BoardCode,
                                           LayoutCode = "L2",
                                           LayoutNumber = 2,
                                           LayoutSequence = 2,
                                           Revisions = new List<BoardRevision>
                                                           {
                                                               new BoardRevision
                                                                   {
                                                                       LayoutCode = "L2",
                                                                       RevisionCode = "L2R1",
                                                                       LayoutSequence = 2
                                                                   }
                                                           }
                                       });

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "CAP 123", AssemblyTechnology = "SM" });
            this.result = this.Sut.UpdateComponents(
                this.BoardCode,
                this.pcasChange,
                this.ChangeRequestId,
                this.componentsToAdd,
                this.componentsToRemove);
        }

        [Test]
        public void ShouldSetToValuesCorrectly()
        {
            this.result.Components.Should().HaveCount(2);
            var addedComponent = this.result.Components.First(a => a.BoardLine == 2);
            addedComponent.ToLayoutVersion.Should().Be(1);
            addedComponent.ToRevisionVersion.Should().BeNull();
        }
    }
}
