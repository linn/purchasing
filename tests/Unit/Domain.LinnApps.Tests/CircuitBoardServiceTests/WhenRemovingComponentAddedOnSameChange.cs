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

    public class WhenRemovingComponentAddedOnSameChange : ContextBase
    {
        private CircuitBoard result;

        private PcasChange pcasChange;

        private int changeRequestId;

        private IEnumerable<BoardComponent> componentsToAdd;

        private IEnumerable<BoardComponent> componentsToRemove;

        private int changeId;

        private ChangeRequest changeRequest;

        [SetUp]
        public void SetUp()
        {
            this.changeId = 890;
            this.changeRequestId = 678;
            this.changeRequest = new ChangeRequest
                                     {
                                         DocumentNumber = this.changeRequestId,
                                         BoardCode = this.BoardCode,
                                         RevisionCode = "L1R1",
                                         ChangeState = "PROPOS"
                                     };
            this.pcasChange = new PcasChange
                                  {
                                      BoardCode = this.BoardCode,
                                      ChangeId = this.changeId,
                                      ChangeRequest = this.changeRequest,
                                      ChangeState = "PROPOS",
                                      DocumentNumber = this.changeRequestId,
                                      RevisionCode = "L1R1"
                                  };
            this.componentsToAdd = null;
            this.Board.Components.Add(new BoardComponent
                                          {
                                              BoardCode = this.BoardCode,
                                              BoardLine = 4,
                                              CRef = "C004",
                                              PartNumber = "CAP 123",
                                              AssemblyTechnology = "SM",
                                              ChangeState = "PROPOS",
                                              FromLayoutVersion = 1,
                                              FromRevisionVersion = 1,
                                              ToLayoutVersion = null,
                                              ToRevisionVersion = null,
                                              AddChangeId = 890,
                                              DeleteChangeId = null,
                                              Quantity = 1
                                          });
            this.componentsToRemove = new List<BoardComponent>
                                       {
                                           new BoardComponent
                                               {
                                                   BoardCode = this.BoardCode,
                                                   BoardLine = 4,
                                                   CRef = "C004",
                                                   PartNumber = "CAP 123",
                                                   AssemblyTechnology = "SM",
                                                   ChangeState = "PROPOS",
                                                   FromLayoutVersion = 1,
                                                   FromRevisionVersion = 1,
                                                   ToLayoutVersion = null,
                                                   ToRevisionVersion = null,
                                                   AddChangeId = 890,
                                                   DeleteChangeId = null,
                                                   Quantity = 1
                                               }
                                       };

            this.ChangeRequestRepository.FindById(this.changeRequestId).Returns(this.changeRequest);
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "CAP 123", AssemblyTechnology = "SM" });
            this.Board.Components.Should().HaveCount(2);
            this.result = this.Sut.UpdateComponents(
                this.BoardCode,
                this.pcasChange,
                this.changeRequestId,
                this.componentsToAdd,
                this.componentsToRemove);
        }

        [Test]
        public void ShouldRemoveComponentCompletely()
        {
            this.result.Components.Should().HaveCount(1);
            var component = this.result.Components.First();
            component.BoardLine.Should().Be(1);
        }
    }
}
