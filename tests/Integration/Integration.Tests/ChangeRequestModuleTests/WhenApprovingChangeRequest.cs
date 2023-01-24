namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenApprovingChangeRequest : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.AuthService
                .HasPermissionFor(AuthorisedAction.ApproveChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var changeRequest = new ChangeRequest
            {
                DocumentType = "CRF",
                DocumentNumber = 1,
                DateEntered = new DateTime(2020, 1, 1),
                EnteredBy = new Employee { Id = 10, FullName = "Barry White" },
                ReasonForChange = "Everything changes",
                DescriptionOfChange = "Great idea",
                ChangeState = "PROPOS",
                BomChanges =
                    new List<BomChange>
                        {
                            new BomChange { ChangeId = 1, BomName = "TOAST 001", ChangeState = "PROPOS" }
                        },
                PcasChanges = new List<PcasChange>
                                  {
                                      new PcasChange
                                          {
                                              ChangeId = 1,
                                              BoardCode = "TOAST",
                                              RevisionCode = "BREAD",
                                              ChangeState = "PROPOS"
                                          }
                                  }
            };
            this.Repository.FindById(1).Returns(changeRequest);

            var request = new ChangeRequestStatusChangeResource { Id = 1, Status = "ACCEPT" };

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
            result.ChangeState.Should().Be("ACCEPT");
            result.BomChanges.Count().Should().Be(1);
            result.PcasChanges.Count().Should().Be(1);
        }
    }
}
