namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenMakingLiveAllChangeRequest : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
            {
                DocumentNumber = 1,
                ChangeState = "ACCEPT",
                BomChanges = new List<BomChange>
                                                {
                                                    new BomChange { ChangeState = "ACCEPT", BomName = "ABOM" },
                                                    new BomChange { ChangeState = "ACCEPT", BomName = "BBOM" },
                                                    new BomChange { ChangeState = "LIVE", BomName = "CBOM" }
                                                },
                PcasChanges = new List<PcasChange>
                                                 {
                                                     new PcasChange { ChangeState = "ACCEPT", BoardCode = "001" },
                                                     new PcasChange { ChangeState = "LIVE", BoardCode = "002" }
                                                 }
            };

            var appliedBy = new Employee { Id = 1, FullName = "Dr Drew" };

            this.Sut.MakeLive(appliedBy, null, null);
        }

        [Test]
        public void ShouldBeLive()
        {
            this.Sut.ChangeState.Should().Be("LIVE");
        }

        [Test]
        public void ShouldMakeLiveBomChangesNotLive()
        {
            this.Sut.BomChanges.Count.Should().Be(3);
            this.Sut.BomChanges.Single(b => b.BomName == "ABOM").ChangeState.Should().Be("LIVE");
            this.Sut.BomChanges.Single(b => b.BomName == "BBOM").ChangeState.Should().Be("LIVE");
            this.Sut.BomChanges.Single(b => b.BomName == "CBOM").ChangeState.Should().Be("LIVE");
        }

        [Test]
        public void ShouldMakeLivePcasChangesNotLive()
        {
            this.Sut.PcasChanges.Count.Should().Be(2);
            this.Sut.PcasChanges.Single(p => p.BoardCode == "001").ChangeState.Should().Be("LIVE");
            this.Sut.PcasChanges.Single(p => p.BoardCode == "002").ChangeState.Should().Be("LIVE");
        }
    }
}
