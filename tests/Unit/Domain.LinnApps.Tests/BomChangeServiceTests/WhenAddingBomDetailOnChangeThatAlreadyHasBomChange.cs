namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingBomDetailOnChangeThatAlreadyHasBomChange : ContextBase
    {
        private Bom bom;

        private BomDetail detail;

        private ChangeRequest request;

        [SetUp]
        public void SetUp()
        {
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
                                                                ChangeState = "PROPOS",
                                                                BomId = 1,
                                                                BomName = "TEST"
                                                            }
                                                    }
            };

            this.Sut.AddBomDetail("TEST", this.request, 7004, 1);
        }

        [Test]
        public void ShouldNotLookupBom()
        {
            this.BomRepository.DidNotReceive().FindById(1);
        }

        [Test]
        public void ShouldNotAddBomChange()
        {
            this.BomChangeRepository.DidNotReceive().Add(Arg.Any<BomChange>());
        }

        [Test]
        public void ShouldAddNewBomDetail()
        {
            this.BomDetailRepository.Received().Add(Arg.Any<BomDetail>());
        }
    }
}
