namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenReplacingBomDetailWithExistingCancelledBomChange : ContextBase
    {
        private Bom bom;

        private BomDetail detail;

        private ChangeRequest request;

        [SetUp]
        public void SetUp()
        {
            this.detail = new BomDetail { DetailId = 1, PartNumber = "OLD 001", Qty = 1, BomId = 1 };
            this.BomDetailRepository.FindById(1).Returns(this.detail);

            this.bom = new Bom { BomId = 1, Part = new Part { PartNumber = "ASS 007" } };
            this.BomRepository.FindById(1).Returns(this.bom);

            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(12);

            this.request = new ChangeRequest
                               {
                                   DocumentNumber = 1,
                                   ChangeState = "PROPOS",
                                   ChangeRequestType = "REPLACE",
                                   OldPart = new Part { PartNumber = "OLD 001" },
                                   NewPart = new Part { PartNumber = "NEW 002" },
                                   BomChanges = new List<BomChange>
                                                    {
                                                        new BomChange
                                                            {
                                                                ChangeId = 7,
                                                                ChangeState = "CANCEL",
                                                                BomId = 1
                                                            }
                                                    }
            };

            this.Sut.ReplaceBomDetail(1, this.request, 7004, 2);
        }

        [Test]
        public void ShouldLookupDetail()
        {
            this.BomDetailRepository.Received().FindById(1);
        }

        [Test]
        public void ShouldLookupBom()
        {
            this.BomRepository.Received().FindById(1);
        }

        [Test]
        public void ShouldAddBomChange()
        {
            this.BomChangeRepository.Received().Add(Arg.Any<BomChange>());
        }

        [Test]
        public void ShouldRemoveOldBomDetail()
        {
            this.detail.DeleteChange.Should().NotBeNull();
            this.detail.DeleteChangeId.Should().Be(12);
            this.detail.DeleteReplaceSeq.Should().Be(1);
        }

        [Test]
        public void ShouldAddNewBomDetail()
        {
            this.BomDetailRepository.Received().Add(Arg.Any<BomDetail>());
        }
    }
}
