namespace Linn.Purchasing.Domain.LinnApps.Tests.PcasChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenProposedAndCheckingCanCancel : ContextBase
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
        public void ShouldBeAbleToBeCancelled()
        {
            this.Sut.CanCancel().Should().BeTrue();
        }
    }
}
