namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCancellingChangeRequestInFull : ContextBase
    {
        private ChangeRequest result;

        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequest
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "PROPOS",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Rubbish Change Request"
                              };
            this.Repository.FindById(1).Returns(request);

            var employee = new Employee { Id = 7, FullName = "Bond, James Bond" };
            this.EmployeeRepository.FindById(7).Returns(employee);

            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.result = this.Sut.Cancel(1, 7, null, null, new List<string>());
        }

        [Test]
        public void ShouldCancelChangeRequest()
        {
            this.result.Should().NotBeNull();
            this.result.ChangeState.Should().Be("CANCEL");
        }
    }
}
