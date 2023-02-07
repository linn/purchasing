namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenLiveAndCheckingCanUndo : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "LIVE"
                           };
        }

        [Test]
        public void ShouldBeUndoable()
        {
            this.Sut.CanUndo().Should().BeTrue();
        }
    }
}
