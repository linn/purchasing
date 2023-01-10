namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    public class WhenPhasingInChangeRequestWithNonExistentWeek : ContextBase
    {
        public class WhenPhasingInNonExistentChangeRequest : ContextBase
        {
            private Action action;

            [SetUp]
            public void SetUp()
            {
                this.AuthService
                    .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                    .Returns(true);

                var bomChangeIds = new List<int> { 1 };

                var request = new ChangeRequest
                                  {
                                      DocumentNumber = 1,
                                      ChangeState = "ACCEPT",
                                      DateEntered = new DateTime(2022, 1, 1),
                                      DescriptionOfChange = "Test Change"
                                  };
                this.Repository.FindById(1).Returns(request);

                this.action = () => this.Sut.PhaseInChanges(1, 665, new List<int>(), new List<string>());
            }

            [Test]
            public void ShouldThrowNotFoundException()
            {
                this.action.Should().Throw<ItemNotFoundException>();
            }
        }
    }
}
