namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingChangeRequestWithReplace : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequestResource
                              {
                                  EnteredBy = new EmployeeResource { Id = 1, FullName = "Jailbird Joe" },
                                  ProposedBy = new EmployeeResource { Id = 2, FullName = "Radical Rick" },
                                  BoardCode = "001",
                                  RevisionCode = "L2R1",
                                  OldPartNumber = "PCAS 001/L1R1",
                                  NewPartNumber = "PCAS 001/L1R2",
                                  GlobalReplace = true,
                                  ReasonForChange = "Cos",
                                  DescriptionOfChange = "Ch Ch Changes"
                              };
            var oldPart = new Part { PartNumber = "PCAS 001/L1R1", Description = "Old Board" };
            var newPart = new Part { PartNumber = "PCAS 001/L1R2", Description = "New Board" };
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(oldPart, newPart);


            var createdRequest = new ChangeRequest
                                     {
                                         DocumentNumber = 24,
                                         ChangeState = "PROPOS",
                                         BoardCode = "001",
                                         RevisionCode = "L2R1",
                                         OldPartNumber = "PCAS 001/L1R1",
                                         NewPartNumber = "PCAS 001/L1R2",
                                         GlobalReplace = "Y",
                                         ReasonForChange = "Cos",
                                         DescriptionOfChange = "Ch Ch Changes"
                                     };
            this.Repository.FindById(24).Returns(createdRequest);

            this.DatabaseService.GetNextVal("CRF_SEQ").Returns(24);

            this.result = this.Sut.AddAndReplace(request, 7004, new List<string> { "superpowers" });
        }

        [Test]
        public void ShouldReturnCreatedRequest()
        {
            this.result.Should().BeOfType<CreatedResult<ChangeRequestResource>>();
        }

        [Test]
        public void ShouldAddToRepository()
        {
            this.Repository.Received().Add(Arg.Any<ChangeRequest>());
        }

        [Test]
        public void ShouldReturnChangeRequestWithNumber()
        {
            var resource = ((CreatedResult<ChangeRequestResource>)this.result).Data;
            resource.ChangeState.Should().Be("PROPOS");
            resource.DocumentNumber.Should().Be(24);
        }

        [Test]
        public void ShouldTryAndReplaceAllOldPart()
        {
            this.BomChangeService.ReplaceAllBomDetails(Arg.Any<ChangeRequest>(), 7004, null);
        }
    }
}
