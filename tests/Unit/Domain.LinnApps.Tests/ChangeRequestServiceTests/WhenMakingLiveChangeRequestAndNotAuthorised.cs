namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenMakingLiveChangeRequestAndNotAuthorised : ContextBase
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

            var employee = new Employee { Id = 7, FullName = "Foreign Spy" };
            this.EmployeeRepository.FindById(7).Returns(employee);

            this.action = () => this.Sut.MakeLive(1, 7, null, null, new List<string>());
        }

        [Test]
        public void ShouldThrowUnauthorisedActionException()
        {
            this.action.Should().Throw<UnauthorisedActionException>();
        }
    }
}
