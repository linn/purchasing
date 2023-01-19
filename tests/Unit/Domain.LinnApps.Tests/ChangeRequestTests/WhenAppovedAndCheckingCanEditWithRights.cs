namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenAppovedAndCheckingCanEditWithRights : ContextBase
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
        public void ShouldBeCancelable()
        {
            this.Sut.CanEdit(true).Should().BeTrue();
        }
    }
}
