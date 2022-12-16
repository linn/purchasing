namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenProposedAndCheckingCanMakeLive : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new BomChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "PROPOS"
                           };
        }

        [Test]
        public void ShouldntBeAbleToBeMakeLive()
        {
            this.Sut.CanMakeLive().Should().BeFalse();
        }
    }
}
