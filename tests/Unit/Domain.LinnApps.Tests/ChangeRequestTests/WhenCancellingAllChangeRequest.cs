namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NUnit.Framework;

    public class WhenCancellingAllChangeRequest : ContextBase
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
                                                    new BomChange { ChangeState = "PROPOS", BomName = "ABOM" },
                                                    new BomChange { ChangeState = "PROPOS", BomName = "BBOM" },
                                                    new BomChange { ChangeState = "LIVE", BomName = "CBOM" }
                                                },
                               PcasChanges = new List<PcasChange>
                                                 {
                                                     new PcasChange { ChangeState = "PROPOS", BoardCode = "001" },
                                                     new PcasChange { ChangeState = "LIVE", BoardCode = "002" }
                                                 }
                            };

            var cancelledBy = new Employee { Id = 1, FullName = "The Great Chancellor" };

            this.Sut.Cancel(cancelledBy, null, null);
        }

        [Test]
        public void ShouldBeCancelled()
        {
            this.Sut.ChangeState.Should().Be("CANCEL");
        }

        [Test]
        public void ShouldCancelBomChangesNotLive()
        {
            this.Sut.BomChanges.Count.Should().Be(3);
            this.Sut.BomChanges.Single(b => b.BomName == "ABOM").ChangeState.Should().Be("CANCEL");
            this.Sut.BomChanges.Single(b => b.BomName == "BBOM").ChangeState.Should().Be("CANCEL");
            this.Sut.BomChanges.Single(b => b.BomName == "CBOM").ChangeState.Should().Be("LIVE");
        }

        [Test]
        public void ShouldCancelPcasChangesNotLive()
        {
            this.Sut.PcasChanges.Count.Should().Be(2);
            this.Sut.PcasChanges.Single(p => p.BoardCode == "001").ChangeState.Should().Be("CANCEL");
            this.Sut.PcasChanges.Single(p => p.BoardCode == "002").ChangeState.Should().Be("LIVE");
        }
    }
}
