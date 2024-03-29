﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenRemovingComponentsAlreadyRemoved : ContextBase
    {
        private IEnumerable<BoardComponent> componentsToAdd;

        private IEnumerable<BoardComponent> componentsToRemove;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.componentsToAdd = null;
            this.componentsToRemove = new List<BoardComponent>
                                       {
                                           new BoardComponent
                                               {
                                                   BoardCode = this.BoardCode,
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
            this.Board.Components.First(a => a.BoardLine == 1).DeleteChangeId = 987;

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "CAP 123", AssemblyTechnology = "SM" });
            this.action = () => this.Sut.UpdateComponents(
                this.BoardCode,
                this.PcasChange,
                this.ChangeRequestId,
                this.componentsToAdd,
                this.componentsToRemove);
        }

        [Test]
        public void ShouldThrowError()
        {
            this.action.Should().Throw<InvalidActionException>();
        }
    }
}
