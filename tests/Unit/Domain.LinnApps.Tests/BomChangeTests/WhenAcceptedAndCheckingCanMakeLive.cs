namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenAcceptedAndCheckingCanMakeLive : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new BomChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "ACCEPT"
                           };
        }

        [Test]
        public void ShouldBeAbleToBeMakeLive()
        {
            this.Sut.CanMakeLive().Should().BeTrue();
        }
    }
}
