namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeTests
{
    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenPhasingInBomChange : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new BomChange
                           {
                               DocumentNumber = 1,
                               ChangeState = "ACCEPT"
                           };
            var week = new LinnWeek() { WeekNumber = 1, WwYyyy = "012022" };
            this.Sut.PhaseIn(week);
        }

        [Test]
        public void ShouldBeCancelled()
        {
            this.Sut.ChangeState.Should().Be("ACCEPT");
            this.Sut.PhaseInWeek.Should().NotBeNull();
            this.Sut.PhaseInWeekNumber.Should().Be(1);
            this.Sut.PhaseInWeek.WeekNumber.Should().Be(1);
        }
    }
}
