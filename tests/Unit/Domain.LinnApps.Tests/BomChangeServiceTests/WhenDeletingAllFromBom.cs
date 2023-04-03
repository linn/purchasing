namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenDeletingAllFromBom : ContextBase
    {
        private BomDetail detail1;

        private BomDetail detail2;

        private BomDetail detail3;

        private BomDetail detail4;

        private BomDetail detail5;

        private BomDetail detail6;

        [SetUp]
        public void SetUp()
        {
            this.detail1 = new BomDetail { DetailId = 1, ChangeState = "PROPOS", AddChange = new BomChange() };
            this.detail2 = new BomDetail { DetailId = 2, ChangeState = "PROPOS", AddChange = new BomChange() };
            this.detail3 = new BomDetail { DetailId = 3, ChangeState = "PROPOS", AddChange = new BomChange() };
            this.detail4 = new BomDetail { DetailId = 4, ChangeState = "PROPOS", AddChange = new BomChange(), DeleteChangeId = 1 };
            this.detail5 = new BomDetail { DetailId = 5, ChangeState = "CANCEL", AddChange = new BomChange() };
            this.detail6 = new BomDetail
                               {
                                   DetailId = 6, AddChange = new BomChange { DocumentNumber = 123 }
                               };


            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>())
                .Returns(new Bom
                             {
                                 BomId = 123, 
                                 Details = new List<BomDetailViewEntry>
                                               {
                                                    new BomDetailViewEntry { DetailId = 1 },
                                                    new BomDetailViewEntry { DetailId = 2 },
                                                    new BomDetailViewEntry { DetailId = 3 },
                                                    new BomDetailViewEntry { DetailId = 4 },
                                                    new BomDetailViewEntry { DetailId = 5 },
                                                    new BomDetailViewEntry { DetailId = 6 }
                                               }
                });
            this.BomDetailRepository.FindById(1).Returns(this.detail1);
            this.BomDetailRepository.FindById(2).Returns(this.detail2);
            this.BomDetailRepository.FindById(3).Returns(this.detail3);
            this.BomDetailRepository.FindById(4).Returns(this.detail4);
            this.BomDetailRepository.FindById(5).Returns(this.detail5);
            this.BomDetailRepository.FindById(6).Returns(this.detail6);

            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(999);
            this.Sut.DeleteAllFromBom("BOM", 123, 33087);
        }

        [Test]
        public void ShouldUpdateNonCancelledNonDeletedDetailsNotAddedByThisChangeRequest()
        {
            this.detail1.DeleteChangeId.Should().Be(999);
            this.detail2.DeleteChangeId.Should().Be(999);
            this.detail3.DeleteChangeId.Should().Be(999);
        }

        [Test]
        public void ShouldNotUpdateCancelledOrDeletedDetails()
        {
            this.detail4.DeleteChangeId.Should().Be(1);
            this.detail5.DeleteChangeId.Should().BeNull();
        }

        [Test]
        public void ShouldDeleteDetailsAddedByThisChangeRequest()
        {
            this.BomDetailRepository.Received()
                .Remove(Arg.Is<BomDetail>(d => d.DetailId == this.detail6.DetailId));
        }
    }
}
