namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenMakingLiveBomChange : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new BomChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "ACCEPT"
                           };
            var employee = new Employee { Id = 1, FullName = "Piers Morgan" };
            this.Sut.MakeLive(employee);
        }

        [Test]
        public void ShouldBeLIVE()
        {
            this.Sut.ChangeState.Should().Be("LIVE");
            this.Sut.DateApplied.Should().NotBeNull();
            this.Sut.AppliedBy.Should().NotBeNull();
            this.Sut.AppliedById.Should().Be(1);
        }
    }
}
