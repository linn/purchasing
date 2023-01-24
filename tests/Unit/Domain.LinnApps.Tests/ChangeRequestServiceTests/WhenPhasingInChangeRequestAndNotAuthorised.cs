namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPhasingInChangeRequestAndNotAuthorised : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequest
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "ACCEPT",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Dastardly Change Request"
                              };
            this.Repository.FindById(1).Returns(request);

            var week = new LinnWeek { WeekNumber = 1, WwYyyy = "012022", EndsOn = new DateTime(2100, 12, 12) };
            this.WeekRepository.FindById(1).Returns(week);

            var bomChangeIds = new List<int> { 1 };

            this.action = () => this.Sut.PhaseInChanges(1, 1, null, new List<int>(), new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
