namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingAlreadyCancelledChangeRequest : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var request = new ChangeRequest
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "CANCEL",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Already Cancelled"
                              };
            this.Repository.FindById(1).Returns(request);

            var employee = new Employee {Id = 7, FullName = "Bond, James Bond"};
            this.EmployeeRepository.FindById(7).Returns(employee);

            this.action = () => this.Sut.Cancel(1, 7, null, null, new List<string>());
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            this.action.Should().Throw<InvalidStateChangeException>();
        }
    }
}
