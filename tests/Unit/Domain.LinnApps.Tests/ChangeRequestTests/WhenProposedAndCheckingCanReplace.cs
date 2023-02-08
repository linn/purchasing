namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenProposedAndCheckingCanReplace : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "PROPOS",
                               ChangeRequestType = "REPLACE"
                           };
        }

        [Test]
        public void ShouldBeAbleToReplace()
        {
            this.Sut.CanReplace(false).Should().BeTrue();
        }
    }
}
