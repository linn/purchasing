namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPhasingInChanges : ContextBase
    {
        private ChangeRequest result;

        [SetUp]
        public void SetUp()
        {
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

            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var bomChangeIds = new List<int> { 1 };
            this.result = this.Sut.PhaseInChanges(1, 1, bomChangeIds, new List<string>());
        }

        [Test]
        public void ShouldPhaseInChangeRequest()
        {
            this.result.Should().NotBeNull();
            this.result.ChangeState.Should().Be("ACCEPT");
            this.result.BomChanges.Count.Should().Be(1);
            this.result.BomChanges.First().PhaseInWeek.Should().NotBeNull();
            this.result.BomChanges.First().PhaseInWeek.WeekNumber.Should().Be(1);
        }
    }
}
