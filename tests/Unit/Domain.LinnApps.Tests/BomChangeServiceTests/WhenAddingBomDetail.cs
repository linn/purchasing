namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingBomDetail : ContextBase
    {
        private Bom bom;

        private BomDetail detail;

        private ChangeRequest request;

        [SetUp]
        public void SetUp()
        {
            this.bom = new Bom { BomId = 1, Part = new Part { PartNumber = "ASS 007" } };
            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(this.bom);

            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(13);

            this.request = new ChangeRequest
                               {
                                   DocumentNumber = 1,
                                   ChangeState = "PROPOS",
                                   ChangeRequestType = "REPLACE",
                                   OldPart = new Part { PartNumber = "OLD 001" },
                                   NewPart = new Part { PartNumber = "NEW 002" },
                                   BomChanges = new List<BomChange>()
                               };


            this.Sut.AddBomDetail("NEW BOM", this.request, 7004, 1);
        }

        [Test]
        public void ShouldLookupBom()
        {
            this.BomRepository.Received().FindBy(Arg.Any<Expression<Func<Bom, bool>>>());
        }

        [Test]
        public void ShouldAddBomChange()
        {
            this.BomChangeRepository.Received().Add(Arg.Any<BomChange>());
        }
        

        [Test]
        public void ShouldAddNewBomDetail()
        {
            this.BomDetailRepository.Received().Add(Arg.Any<BomDetail>());
        }
    }
}
