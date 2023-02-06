namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Resources;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Net.Http.Json;
    using System.Net;
    using System;
    using System.Linq;

    public class WhenUndoingChangeRequest : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var changeRequest = new ChangeRequest
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
                                                                  ChangeId = 1, BomName = "TOAST 001", ChangeState = "ACCEPT"
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
                                                                            ChangeState = "ACCEPT"
                                                                        }
                                                                }
                                          };

            // return undoneChangeRequest on second call
            this.Repository.FindById(1).Returns(changeRequest, undoneChangeRequest);

            var employee = new Employee() { Id = 1, FullName = "Alison" };
            this.EmployeeRepository.FindById(Arg.Any<int>()).Returns(employee);

            var request = new ChangeRequestStatusChangeResource { Id = 1, Status = "UNDO", SelectedBomChangeIds = new List<int> { 1 } };

            this.Response = this.Client.PostAsJsonAsync(
                "/purchasing/change-requests/status", request).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldCallRepository()
        {
            this.Repository.Received().FindById(1);
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var result = this.Response.DeserializeBody<ChangeRequestResource>();
            result.Should().NotBeNull();
            result.DocumentNumber.Should().Be(1);
            result.ChangeState.Should().Be("LIVE");
            result.BomChanges.Count().Should().Be(2);
            result.PcasChanges.Count().Should().Be(1);
        }
    }
}
