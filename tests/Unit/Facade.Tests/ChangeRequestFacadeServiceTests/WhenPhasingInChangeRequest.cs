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

    public class WhenPhasingInChangeRequest : ContextBase
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
                                  DocumentNumber = 1,
                                  ChangeState = "ACCEPT",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Test Change",
                                  BomChanges = new List<BomChange>
                                                   {
                                                       new BomChange { ChangeId = 1, ChangeState = "ACCEPT" }
                                                   }
                              };
            this.Repository.FindById(1).Returns(request);

            var week = new LinnWeek { WeekNumber = 1, WwYyyy = "012022" };
            this.WeekRepository.FindById(1).Returns(week);

            var phaseInRequest = new ChangeRequestPhaseInsResource { DocumentNumber = 1, PhaseInWeek = 1, SelectedBomChangeIds = new List<int> { 1 } };
            this.result = this.Sut.PhaseInChangeRequest(phaseInRequest, null);
        }

        [Test]
        public void ShouldReturnSuccessRequest()
        {
            this.result.Should().BeOfType<SuccessResult<ChangeRequestResource>>();
        }

        [Test]
        public void ShouldLeaveChangeStatusAtAccepted()
        {
            var resource = ((SuccessResult<ChangeRequestResource>)this.result).Data;
            resource.DocumentNumber.Should().Be(1);
            resource.ChangeState.Should().Be("ACCEPT");
        }

        [Test]
        public void ShouldPhaseInBomChange()
        {
            var resource = ((SuccessResult<ChangeRequestResource>)this.result).Data;
            var change = resource.BomChanges.FirstOrDefault();
            change.Should().NotBeNull();
            change.PhaseInWeekNumber.Should().Be(1);
            change.PhaseInWWYYYY.Should().Be("012022");
        }
    }
}
