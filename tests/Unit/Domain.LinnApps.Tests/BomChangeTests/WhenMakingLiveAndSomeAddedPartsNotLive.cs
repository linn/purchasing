namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NUnit.Framework;

    public class WhenMakingLiveAndSomeAddedPartsNotLive : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new BomChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "ACCEPT",
                               AddedBomDetails = new[]
                                                     {
                                                         new BomDetail { Part = new Part { DateLive = null } }
                                                     }
                           };
            var employee = new Employee { Id = 1, FullName = "Piers Morgan" };
            this.Sut.MakeLive(employee);
        }

        [Test]
        public void ShouldNotBeLive()
        {
            this.Sut.ChangeState.Should().Be("ACCEPT");
            this.Sut.DateApplied.Should().BeNull();
            this.Sut.AppliedBy.Should().BeNull();
            this.Sut.AppliedById.Should().BeNull();
        }
    }
}
