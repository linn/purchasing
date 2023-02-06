namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenLiveAndCheckingCanUndo : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new BomChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "LIVE",
                               PcasChange = "N"
                           };
        }

        [Test]
        public void ShouldBeAbleToBeUndo()
        {
            this.Sut.CanUndo().Should().BeTrue();
        }
    }
}
