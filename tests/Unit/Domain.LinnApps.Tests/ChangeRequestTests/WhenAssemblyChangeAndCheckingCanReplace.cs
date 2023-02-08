namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenAssemblyChangeAndCheckingCanReplace : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "PROPOS",
                               ChangeRequestType = "PARTEDIT"
                           };
        }

        [Test]
        public void ShouldNotBeCancelable()
        {
            this.Sut.CanReplace(true).Should().BeFalse();
        }
    }
}
