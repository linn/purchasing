namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenCancellingChangeRequest : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "PROPOS"
                           };

            this.Sut.CancelAll();
        }

        [Test]
        public void ShouldBeApproved()
        {
            this.Sut.ChangeState.Should().Be("CANCEL");
        }
    }
}
