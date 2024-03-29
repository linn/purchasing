﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.CircuitBoardServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingComponentsWithoutPcasChange : ContextBase
    {
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
                                      ChangeRequest = null, 
                                      ChangeState = null,
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

            this.BoardRepository.FindById(this.BoardCode).Returns(this.Board);
            this.ChangeRequestRepository.FindById(this.changeRequestId).Returns(this.changeRequest);
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "CAP 123", AssemblyTechnology = "SM" });
            this.Sut.UpdateComponents(
                this.BoardCode,
                this.pcasChange,
                this.changeRequestId,
                this.componentsToAdd,
                this.componentsToRemove);
        }

        [Test]
        public void ShouldPopulatePcasChange()
        {
            this.pcasChange.DocumentNumber.Should().Be(this.changeRequest.DocumentNumber);
            this.pcasChange.DocumentType.Should().Be(this.changeRequest.DocumentType);
            this.pcasChange.ChangeRequest.Should().Be(this.changeRequest);
            this.pcasChange.ChangeState.Should().Be(this.changeRequest.ChangeState);
        }
    }
}
