namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenReplaceOnChangeRequest : ContextBase
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
                                  BomChanges = new List<BomChange>()
                              };
            this.Repository.FindById(1).Returns(request);

            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var detailIds = new List<int> { 1, 2 };
            this.result = this.Sut.Replace(1, 7004, false, false, null, detailIds, null, new List<string>());
        }

        [Test]
        public void ShouldReturnChangeRequest()
        {
            this.result.Should().NotBeNull();
            this.result.ChangeState.Should().Be("ACCEPT");
        }

        [Test]
        public void ShouldCallReplaceonSelectedDetailIds()
        {
            this.BomChangeService.Received().ReplaceBomDetail(1, Arg.Any<ChangeRequest>(), 7004, null);
            this.BomChangeService.Received().ReplaceBomDetail(2, Arg.Any<ChangeRequest>(), 7004, null);
        }
    }
}
