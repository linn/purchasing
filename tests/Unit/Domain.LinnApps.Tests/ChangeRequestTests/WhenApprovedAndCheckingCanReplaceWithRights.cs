namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NUnit.Framework;

    public class WhenApprovedAndCheckingCanReplaceWithRights : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "ACCEPT",
                               ChangeRequestType = "REPLACE",
                               OldPart = new Part { PartNumber = "OLD" },
                               NewPart = new Part { PartNumber = "NEW" }
                           };
        }

        [Test]
        public void ShouldBeAbleToReplace()
        {
            this.Sut.CanReplace(true).Should().BeTrue();
        }
    }
}
