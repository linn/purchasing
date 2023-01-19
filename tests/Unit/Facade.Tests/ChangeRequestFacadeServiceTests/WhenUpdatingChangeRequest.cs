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

    public class WhenUpdatingChangeRequest : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            var updateResource = new ChangeRequestResource
                              {
                                  DocumentNumber = 1,
                                  EnteredBy = new EmployeeResource { Id = 1, FullName = "Matt Hancock" },
                                  ProposedBy = new EmployeeResource { Id = 2, FullName = "Bobby Davro" },
                                  NewPartNumber = "CONN 1",
                                  ReasonForChange = "Known",
                                  DescriptionOfChange = "Deleketable the halls"
            };

            var request = new ChangeRequest
                              {
                                  DocumentNumber = 1,
                                  DocumentType = "CRF",
                                  NewPartNumber = "CONN 1",
                                  ReasonForChange = "Its a mystery",
                                  DescriptionOfChange = "Deck the halls",
                                  ChangeState = "ACCEPT"
                              };
            this.Repository.FindById(1).Returns(request);

            this.AuthorisationService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.result = this.Sut.Update(1, updateResource, new List<string> { "superpowers" });
        }

        [Test]
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<ChangeRequestResource>>();
        }

        [Test]
        public void ShouldCommitChanges()
        {
            this.TransactionManager.Received().Commit();
        }

        [Test]
        public void ShouldReturnChangeRequestWithChangedFields()
        {
            var resource = ((SuccessResult<ChangeRequestResource>)this.result).Data;
            resource.ChangeState.Should().Be("ACCEPT");
            resource.DocumentNumber.Should().Be(1);
            resource.NewPartNumber.Should().Be("CONN 1");
            resource.ReasonForChange.Should().Be("Known");
            resource.DescriptionOfChange.Should().Be("Deleketable the halls");
        }
    }
}
