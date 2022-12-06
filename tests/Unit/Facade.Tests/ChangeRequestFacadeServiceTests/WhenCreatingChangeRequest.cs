namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests 
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingChangeRequest : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequestResource
                              {
                                  EnteredBy = new EmployeeResource { Id = 1, FullName = "Matt Hancock" },
                                  ProposedBy = new EmployeeResource { Id = 2, FullName = "Bobby Davro" },
                                  NewPartNumber = "CONN 1",
                                  ReasonForChange = "Cos",
                                  DescriptionOfChange = "Ch Ch Changes"
                              };
            var part = new Part {PartNumber = "CONN 1", Description = "A Connector" };

            this.databaseService.GetNextVal("CRF_SEQ").Returns(42);
            this.partRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>()).Returns(part);
            this.result = this.Sut.Add(request, new List<string>() { "superpowers" });
        }

        [Test]
        public void ShouldReturnCreatedRequest()
        {
            this.result.Should().BeOfType<CreatedResult<ChangeRequestResource>>();
        }

        [Test]
        public void ShouldAddToRepository()
        {
            this.repository.Received().Add(Arg.Any<ChangeRequest>());
        }

        [Test]
        public void ShouldReturnChangeRequestWithNumber()
        {
            var resource = ((CreatedResult<ChangeRequestResource>)this.result).Data;
            resource.ChangeState.Should().Be("PROPOS");
            resource.DocumentNumber.Should().Be(42);
            resource.NewPartNumber.Should().Be("CONN 1");
        }
    }
}
