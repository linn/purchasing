namespace Linn.Purchasing.Domain.LinnApps.Tests.PcasChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenLiveAndCheckingCanCancel : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new PcasChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "PROPOS"
                           };
        }

        [Test]
        public void ShouldntBeAbleToBeCancelled()
        {
            this.Sut.CanCancel().Should().BeTrue();
        }
    }
}
