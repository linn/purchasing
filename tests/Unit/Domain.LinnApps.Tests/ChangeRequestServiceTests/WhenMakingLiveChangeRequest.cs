namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenMakingLiveChangeRequest : ContextBase
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
                                  DescriptionOfChange = "Brilliant Change Request"
                              };
            this.Repository.FindById(1).Returns(request);

            var employee = new Employee { Id = 7, FullName = "Dougie Howser" };
            this.EmployeeRepository.FindById(7).Returns(employee);

            this.AuthService
                .HasPermissionFor(AuthorisedAction.MakeLiveChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.result = this.Sut.MakeLive(1, 7, null, null, new List<string>());
        }

        [Test]
        public void ShouldMakeLiveChangeRequest()
        {
            this.result.Should().NotBeNull();
            this.result.ChangeState.Should().Be("LIVE");
        }
    }
}
