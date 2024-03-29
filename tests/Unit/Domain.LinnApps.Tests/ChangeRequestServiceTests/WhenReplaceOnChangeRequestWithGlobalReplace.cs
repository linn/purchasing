﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenReplaceOnChangeRequestWithGlobalReplace : ContextBase
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
                                  BomChanges = new List<BomChange>(),
                                  OldPartNumber = "OLD 1",
                                  NewPartNumber = "NEW 1"
                              };
            this.Repository.FindById(1).Returns(request);

            this.AuthService
                .HasPermissionFor(AuthorisedAction.AdminChangeRequest, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            var detailIds = new List<int> { 1, 2 };
            this.result = this.Sut.Replace(1, 7004, true, true, null, null, null, null, new List<string>());
        }

        [Test]
        public void ShouldReturnChangeRequest()
        {
            this.result.Should().NotBeNull();
            this.result.ChangeState.Should().Be("ACCEPT");
        }

        [Test]
        public void ShouldCallReplaceAllBomDetails()
        {
            this.BomChangeService.Received().ReplaceAllBomDetails(Arg.Any<ChangeRequest>(), 7004, null);
        }

        [Test]
        public void ShouldSetGlobalReplace()
        {
            this.result.GlobalReplace.Should().Be("Y");
        }

        [Test]
        public void ShouldCallPcasPackToReplaceComponents()
        {
            this.PcasPack.Received().ReplaceAll("OLD 1", 1, "ACCEPT", 7004, "NEW 1");
        }
    }
}
