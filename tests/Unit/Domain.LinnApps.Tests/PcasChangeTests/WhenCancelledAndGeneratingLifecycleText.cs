namespace Linn.Purchasing.Domain.LinnApps.Tests.PcasChangeTests
{
    using System;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenCancelledAndGeneratingLifecycleText : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new PcasChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "CANCEL",
                               DateCancelled = new DateTime(2022, 12, 24),
                               CancelledBy = new Employee { Id = 3, FullName = "King Herod" }
                           };
        }

        [Test]
        public void ShouldGenerateRightText()
        {
            this.Sut.LifecycleText().Should().Be("Cancelled on 24-Dec-22 by King Herod");
        }
    }
}
