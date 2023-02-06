namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUndoingChangeRequest : ContextBase
    {
        private IResult<ChangeRequestResource> result;

        [SetUp]
        public void SetUp()
        {
            this.AuthorisationService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var request = new ChangeRequest
            {
                DocumentType = "CRF",
                DocumentNumber = 1,
                DateEntered = new DateTime(2020, 1, 1),
                EnteredBy = new Employee { Id = 10, FullName = "Sue Ellen" },
                ReasonForChange = "Everything changes",
                DescriptionOfChange = "Great idea",
                ChangeState = "LIVE",
                BomChanges =
                                            new List<BomChange>
                                                {
                                                    new BomChange
                                                        {
                                                            ChangeId = 1, BomName = "TOAST 001", ChangeState = "LIVE"
                                                        }
                                                },
                PcasChanges = new List<PcasChange>
                                                          {
                                                              new PcasChange
                                                                  {
                                                                      ChangeId = 1,
                                                                      BoardCode = "TOAST",
                                                                      RevisionCode = "BREAD",
                                                                      ChangeState = "LIVE"
                                                                  }
                                                          }
            };

            // undo makes new bom change in PL/SQL
            var undoneChangeRequest = new ChangeRequest
            {
                DocumentType = "CRF",
                DocumentNumber = 1,
                DateEntered = new DateTime(2020, 1, 1),
                EnteredBy = new Employee { Id = 10, FullName = "Sue Ellen" },
                ReasonForChange = "Everything changes",
                DescriptionOfChange = "Great idea",
                ChangeState = "LIVE",
                BomChanges =
                                                  new List<BomChange>
                                                      {
                                                          new BomChange
                                                              {
                                                                  ChangeId = 1, BomName = "TOAST 001", ChangeState = "LIVE"
                                                              },
                                                          new BomChange
                                                              {
                                                                  ChangeId = 1, BomName = "TOAST 001", ChangeState = "ACCEPT", Comments = "To undo change 1"
                                                              }
                                                      },
                PcasChanges = new List<PcasChange>
                                                                {
                                                                    new PcasChange
                                                                        {
                                                                            ChangeId = 1,
                                                                            BoardCode = "TOAST",
                                                                            RevisionCode = "BREAD",
                                                                            ChangeState = "LIVE"
                                                                        }
                                                                }
            };
            this.Repository.FindById(1).Returns(request, undoneChangeRequest);

            var employee = new Employee { Id = 1, FullName = "Moana" };
            this.EmployeeRepository.FindById(1).Returns(employee);

            this.result = this.Sut.UndoChangeRequest(1, 1, new List<int> { 1 }, null, null);
        }

        [Test]
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<ChangeRequestResource>>();
        }

        [Test]
        public void ShouldHaveUndoneBomChanges()
        {
            var resource = ((SuccessResult<ChangeRequestResource>)this.result).Data;
            resource.DocumentNumber.Should().Be(1);
            resource.ChangeState.Should().Be("LIVE");
            resource.BomChanges.Count().Should().Be(2);
        }
    }
}
