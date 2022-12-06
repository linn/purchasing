namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenProposedAndCheckingCanCancel : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "PROPOS"
                           };
        }

        [Test]
        public void ShouldBeCancelable()
        {
            this.Sut.CanCancel(false).Should().BeTrue();
        }
    }
}
