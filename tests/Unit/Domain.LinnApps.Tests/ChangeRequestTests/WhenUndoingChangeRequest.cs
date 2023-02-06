namespace Linn.Purchasing.Domain.LinnApps.Tests.ChangeRequestTests
{
    using FluentAssertions;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using NUnit.Framework;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    using NSubstitute;

    public class WhenUndoingChangeRequest : ContextBase
    {
        protected IBomPack BomPack { get; private set; }

        protected IPcasPack PcasPack { get; private set; }

        protected bool Result { get; private set; }

        [SetUp]
        public void SetUp()
        {
            this.Sut = new ChangeRequest
                           {
                               DocumentNumber = 1,
                               ChangeState = "LIVE",
                               BomChanges = new List<BomChange>
                                                {
                                                    new BomChange { ChangeId = 1, ChangeState = "LIVE", BomName = "ABOM" },
                                                    new BomChange { ChangeId = 2, ChangeState = "LIVE", BomName = "BBOM" },
                                                    new BomChange { ChangeId = 3, ChangeState = "LIVE", BomName = "CBOM" }
                                                },
                               PcasChanges = new List<PcasChange>
                                                 {
                                                     new PcasChange { ChangeId = 1, ChangeState = "LIVE", BoardCode = "001" },
                                                     new PcasChange { ChangeId = 2, ChangeState = "LIVE", BoardCode = "002" }
                                                 }
                           };

            var undoneBy = new Employee { Id = 1, FullName = "Liz Truss" };
            var undoneBomChangeIds = new List<int> { 1 };
            var undonePcasChangeIds = new List<int> { 1 };

            this.BomPack = Substitute.For<IBomPack>();
            this.PcasPack = Substitute.For<IPcasPack>();

            this.Result = this.Sut.UndoChanges(
                undoneBy,
                undoneBomChangeIds,
                undonePcasChangeIds,
                this.BomPack,
                this.PcasPack);
        }

        [Test]
        public void ShouldReturnTrue()
        {
            this.Result.Should().BeTrue();
        }

        [Test]
        public void ShouldUndoOneBomChange()
        {
            this.BomPack.Received().UndoBomChange(1,1);
            this.BomPack.DidNotReceive().UndoBomChange(2, 1);
            this.BomPack.DidNotReceive().UndoBomChange(3, 1);
        }

        [Test]
        public void ShouldUndoOnePcasChange()
        {
            this.PcasPack.Received().UndoPcasChange(1, 1);
            this.PcasPack.DidNotReceive().UndoPcasChange(2, 1);
        }
    }
}
