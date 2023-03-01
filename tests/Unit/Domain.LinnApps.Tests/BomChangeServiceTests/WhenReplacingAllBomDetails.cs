namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenREplacingAllBomDetails : ContextBase
    {
        private ChangeRequest request;

        [SetUp]
        public void SetUp()
        {
            var detail1 = new BomDetail { DetailId = 1, PartNumber = "OLD 001", Qty = 1, BomId = 1 };
            var detail2 = new BomDetail { DetailId = 2, PartNumber = "OLD 001", Qty = 1, BomId = 2 };
            var detail3 = new BomDetail { DetailId = 3, PartNumber = "OLD 001", Qty = 1, BomId = 3 };
            this.BomDetailRepository.FindById(1).Returns(detail1);
            this.BomDetailRepository.FindById(2).Returns(detail2);
            this.BomDetailRepository.FindById(3).Returns(detail3);

            var details = new List<BomDetail> { detail1, detail2, detail3 };
            this.BomDetailRepository.FilterBy(Arg.Any <Expression<Func<BomDetail, bool>>>()).Returns(details.AsQueryable());

            var bom1 = new Bom { BomId = 1, Part = new Part { PartNumber = "ASS 007" } };
            var bom2 = new Bom { BomId = 2, Part = new Part { PartNumber = "ASS 007" } };
            var bom3 = new Bom { BomId = 3, Part = new Part { PartNumber = "ASS 007" } };
            this.BomRepository.FindById(1).Returns(bom1);
            this.BomRepository.FindById(2).Returns(bom2);
            this.BomRepository.FindById(3).Returns(bom3);

            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(12);

            this.request = new ChangeRequest
                               {
                                   DocumentNumber = 1,
                                   ChangeState = "PROPOS",
                                   ChangeRequestType = "REPLACE",
                                   OldPart = new Part { PartNumber = "OLD 001" },
                                   NewPart = new Part { PartNumber = "NEW 002" }
                               };



            this.Sut.ReplaceAllBomDetails(request, 7004, null);
        }

        [Test]
        public void ShouldLookupDetails()
        {
            this.BomDetailRepository.Received().FindById(1);
            this.BomDetailRepository.Received().FindById(2);
            this.BomDetailRepository.Received().FindById(3);
        }

        [Test]
        public void ShouldLookupBom()
        {
            this.BomRepository.Received().FindById(1);
            this.BomRepository.Received().FindById(2);
            this.BomRepository.Received().FindById(3);
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
