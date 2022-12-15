namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenApprovedAndCheckingCanMakeLive : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "ACCEPT"
                           };
        }

        [Test]
        public void ShouldBeAbleToMakeLive()
        {
            this.Sut.CanMakeLive().Should().BeTrue();
        }
    }
}
