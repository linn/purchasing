namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenProposedAndCheckingCanPhaseIn : ContextBase
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
        public void ShouldntBeAbleToPhaseIn()
        {
            this.Sut.CanPhaseIn().Should().BeFalse();
        }
    }
}
