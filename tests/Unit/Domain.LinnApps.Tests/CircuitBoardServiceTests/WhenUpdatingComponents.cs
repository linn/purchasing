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
        private CircuitBoard result;

        private IEnumerable<BoardComponent> componentsToAdd;

        private IEnumerable<BoardComponent> componentsToRemove;

        [SetUp]
        public void SetUp()
        {
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
        public void ShouldLookUpBoard()
        {
            this.BoardRepository.Received().FindById(this.BoardCode);
        }

        [Test]
        public void ShouldLookUpChangeRequest()
        {
            this.ChangeRequestRepository.Received().FindById(this.ChangeRequestId);
        }

        [Test]
        public void ShouldUpdateComponents()
        {
            this.result.Components.Should().HaveCount(2);
            var removed = this.result.Components.First(a => a.BoardLine == 1);
            removed.DeleteChangeId.Should().Be(this.ChangeId);
            var added = this.result.Components.First(a => a.BoardLine == 2);
            added.AddChangeId.Should().Be(this.ChangeId);
            added.AssemblyTechnology.Should().Be("SM");
            added.ToLayoutVersion.Should().BeNull();
            added.ToRevisionVersion.Should().BeNull();
        }
    }
}
