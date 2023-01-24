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

    public class WhenRemovingComponentWithSingleRevision : ContextBase
    {
        private CircuitBoard result;

        private IEnumerable<BoardComponent> componentsToAdd;

        private IEnumerable<BoardComponent> componentsToRemove;

        [SetUp]
        public void SetUp()
        {
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
                                                                       LayoutSequence = 2,
                                                                       VersionNumber = 1
                                                                   }
                                                           }
                                       });
            this.Board.Components.Add(new BoardComponent
                                          {
                                              BoardCode = this.BoardCode,
                                              BoardLine = 2,
                                              CRef = "C001",
                                              PartNumber = "CAP 123",
                                              AssemblyTechnology = "SM",
                                              ChangeState = "LIVE",
                                              FromLayoutVersion = 1,
                                              FromRevisionVersion = 1,
                                              ToLayoutVersion = 1,
                                              ToRevisionVersion = 1,
                                              AddChangeId = 123,
                                              DeleteChangeId = null,
                                              Quantity = 1
                                          });
            this.componentsToAdd = null;
            this.componentsToRemove = new List<BoardComponent>
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
                                                      ToLayoutVersion = 1,
                                                      ToRevisionVersion = 1,
                                                      AddChangeId = 123,
                                                      DeleteChangeId = null,
                                                      Quantity = 1
                                                  }
                                          };

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "CAP 123", AssemblyTechnology = "SM" });
            this.result = this.Sut.UpdateComponents(
                this.BoardCode,
                this.PcasChange,
                this.ChangeRequestId,
                this.componentsToAdd,
                this.componentsToRemove);
        }

        [Test]
        public void ShouldMarkAsDeletedAndAddNothing()
        {
            this.result.Components.Should().HaveCount(2);
            var removed = this.result.Components.First(a => a.BoardLine == 2);
            removed.DeleteChangeId.Should().Be(this.ChangeId);
        }
    }
}
