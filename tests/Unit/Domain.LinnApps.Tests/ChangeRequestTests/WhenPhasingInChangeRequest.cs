namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenPhasingInChangeRequest : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            var thisWeek = new LinnWeek { WeekNumber = 1, WwYyyy = "1/2022"};
            var nextWeek = new LinnWeek { WeekNumber = 2, WwYyyy = "2/2022" };

            this.Sut = new ChangeRequest
            {
                DocumentNumber = 1,
                ChangeState = "ACCEPT",
                BomChanges = new List<BomChange>
                                                {
                                                    new BomChange { ChangeId = 1, ChangeState = "ACCEPT", BomName = "ABOM" },
                                                    new BomChange { ChangeId = 2, ChangeState = "ACCEPT", BomName = "BBOM", PhaseInWeek = thisWeek, PhaseInWeekNumber = thisWeek.WeekNumber },
                                                    new BomChange { ChangeId = 3, ChangeState = "LIVE", BomName = "CBOM" },
                                                    new BomChange { ChangeId = 4, ChangeState = "ACCEPT", BomName = "DBOM" }
                                                },
                PcasChanges = new List<PcasChange>
                                                 {
                                                     new PcasChange { ChangeId = 1, ChangeState = "ACCEPT", BoardCode = "001" },
                                                     new PcasChange { ChangeId = 1, ChangeState = "LIVE", BoardCode = "002" }
                                                 }
            };

            var phaseInChanges = new List<int> { 1, 2 };

            this.Sut.PhaseIn(nextWeek, phaseInChanges);
        }

        [Test]
        public void ShouldStillBeAccept()
        {
            this.Sut.ChangeState.Should().Be("ACCEPT");
        }

        [Test]
        public void ShouldMakeSomeBomChangesPhasedInNextWeek()
        {
            this.Sut.BomChanges.Count.Should().Be(4);
            this.Sut.BomChanges.Single(b => b.BomName == "ABOM").PhaseInWeek.Should().NotBeNull();
            this.Sut.BomChanges.Single(b => b.BomName == "ABOM").PhaseInWeek.WeekNumber.Should().Be(2);
            this.Sut.BomChanges.Single(b => b.BomName == "BBOM").PhaseInWeek.Should().NotBeNull();
            this.Sut.BomChanges.Single(b => b.BomName == "BBOM").PhaseInWeek.WeekNumber.Should().Be(2);
            this.Sut.BomChanges.Single(b => b.BomName == "CBOM").ChangeState.Should().Be("LIVE");
            this.Sut.BomChanges.Single(b => b.BomName == "CBOM").PhaseInWeek.Should().BeNull();
            this.Sut.BomChanges.Single(b => b.BomName == "DBOM").PhaseInWeek.Should().BeNull();
        }
    }
}
