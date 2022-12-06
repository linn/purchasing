﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenLiveAndCheckingCanCancel : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "LIVE"
                           };
        }

        [Test]
        public void ShouldNotBeCancelable()
        {
            this.Sut.CanCancel(false).Should().BeFalse();
        }
    }
}
