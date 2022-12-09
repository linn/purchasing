namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using System;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenLiveAndGettingLifecycleText : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new BomChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "LIVE",
                               DateApplied = new DateTime(1752, 1, 1),
                               AppliedBy = new Employee { Id = 3, FullName = "Benny Franklin" }
                           };
        }

        [Test]
        public void ShouldGenerateRightText()
        {
            this.Sut.LifecycleText().Should().Be("Live on 01-Jan-52 by Benny Franklin");
        }
    }
}
