namespace Linn.Purchasing.Integration.Tests.ChangeRequestModuleTests
{
    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Net.Http.Json;
    using System.Net;
    using System;
    using System.Linq;

    public class WhenPhasingInChangeRequest : ContextBase
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
                EnteredBy = new Employee { Id = 10, FullName = "Barry White" },
                ReasonForChange = "Everything changes",
                DescriptionOfChange = "Great idea",
                ChangeState = "ACCEPT",
                BomChanges = new List<BomChange>
                                                  {
                                                      new BomChange
                                                          {
                                                              ChangeId = 1,
                                                              BomName = "TOAST 001",
                                                              ChangeState = "ACCEPT"
                                                          }
                                                  }
            };
            this.Repository.FindById(1).Returns(changeRequest);

            var week = new LinnWeek { WeekNumber = 1, WwYyyy = "012022" };
            this.WeekRepository.FindById(Arg.Any<int>()).Returns(week);

            var request = new ChangeRequestPhaseInsResource { DocumentNumber = 1, PhaseInWeek = 1, SelectedBomChangeIds = new List<int> { 1 } };

            this.Response = this.Client.PostAsJsonAsync(
                "/purchasing/change-requests/phase-ins", request).Result;
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
            var change = result.BomChanges.FirstOrDefault();
            change.Should().NotBeNull();
            change.PhaseInWWYYYY.Should().Be("012022");
        }
    }
}
