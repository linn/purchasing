namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenCancelledAndCheckingCanUndo : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "CANCEL"
                           };
        }

        [Test]
        public void ShouldNotBeUndoable()
        {
            this.Sut.CanUndo().Should().BeFalse();
        }
    }
}
