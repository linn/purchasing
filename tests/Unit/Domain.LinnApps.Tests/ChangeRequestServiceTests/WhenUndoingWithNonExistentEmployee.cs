namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUndoingWithNonExistentEmployee : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var request = new ChangeRequest
                              {
                                  DocumentNumber = 1,
                                  ChangeState = "LIVE",
                                  DateEntered = new DateTime(2022, 1, 1),
                                  DescriptionOfChange = "Unnessary Change Request",
                                  BomChanges = new List<BomChange>
                                                   {
                                                       new BomChange
                                                           {
                                                               ChangeId = 1,
                                                               ChangeState = "LIVE"
                                                           },
                                                       new BomChange
                                                           {
                                                               ChangeId = 2,
                                                               ChangeState = "CANCEL"
                                                           }
                                                   }
                              };
            this.Repository.FindById(1).Returns(request);

            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var bomChangeIds = new List<int> { 1 };

            this.action = () => this.Sut.UndoChanges(1, 7, bomChangeIds, new List<int>(), new List<string>());
        }

        [Test]
        public void ShouldThrowNotFoundException()
        {
            this.action.Should().Throw<ItemNotFoundException>();
        }
    }
}
