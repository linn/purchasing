namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenProposedAndCheckingCanEdit : ContextBase
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
        public void ShouldBeEditable()
        {
            this.Sut.CanEdit(false).Should().BeTrue();
        }
    }
}
