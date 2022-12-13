namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenCancellingRemainderOfChangeRequest : ContextBase
    {
        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
            {
                DocumentNumber = 1,
                ChangeState = "PROPOS",
                BomChanges = new List<BomChange>
                                                {
                                                    new BomChange { ChangeId = 1, ChangeState = "CANCEL", BomName = "ABOM" },
                                                    new BomChange { ChangeId = 2, ChangeState = "CANCEL", BomName = "BBOM" },
                                                    new BomChange { ChangeId = 3, ChangeState = "LIVE", BomName = "CBOM" }
                                                },
                PcasChanges = new List<PcasChange>
                                                 {
                                                     new PcasChange { ChangeId = 1, ChangeState = "CANCEL", BoardCode = "001" },
                                                     new PcasChange { ChangeId = 2, ChangeState = "PROPOS", BoardCode = "002" }
                                                 }
            };

            var cancelledBy = new Employee { Id = 1, FullName = "The Great Chancellor" };
            var cancelPcasIds = new List<int> { 2 };

            this.Sut.Cancel(cancelledBy, null, cancelPcasIds);
        }

        [Test]
        public void ShouldBeCancelled()
        {
            this.Sut.ChangeState.Should().Be("CANCEL");
        }

        [Test]
        public void ShouldLeaveBomChangesAlone()
        {
            this.Sut.BomChanges.Count.Should().Be(3);
            this.Sut.BomChanges.Single(b => b.BomName == "ABOM").ChangeState.Should().Be("CANCEL");
            this.Sut.BomChanges.Single(b => b.BomName == "BBOM").ChangeState.Should().Be("CANCEL");
            this.Sut.BomChanges.Single(b => b.BomName == "CBOM").ChangeState.Should().Be("LIVE");
        }

        [Test]
        public void ShouldCancelRemainderOfPcasChanges()
        {
            this.Sut.PcasChanges.Count.Should().Be(2);
            this.Sut.PcasChanges.Single(p => p.BoardCode == "001").ChangeState.Should().Be("CANCEL");
            this.Sut.PcasChanges.Single(p => p.BoardCode == "002").ChangeState.Should().Be("CANCEL");
        }
    }
}
