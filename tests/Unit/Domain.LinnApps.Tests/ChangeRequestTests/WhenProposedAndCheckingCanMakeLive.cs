﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenProposedAndCheckingCanMakeLive : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "ACCEPT"
                           };
        }

        [Test]
        public void ShouldNotBeApprovable()
        {
            this.Sut.CanApprove().Should().BeFalse();
        }
    }
}
