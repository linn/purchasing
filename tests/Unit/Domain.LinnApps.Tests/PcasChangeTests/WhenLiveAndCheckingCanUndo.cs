namespace Linn.Purchasing.Domain.LinnApps.Tests.PcasChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenLiveAndCheckingCanUndo : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new PcasChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "LIVE"
                           };
        }

        [Test]
        public void ShouldBeAbleToUndo()
        {
            this.Sut.CanUndo().Should().BeTrue();
        }
    }
}
