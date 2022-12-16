﻿namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenReplacingPartOnBom : ContextBase
    {
        private BomTreeNode newTree;

        private BomTreeNode c1;

        private BomTreeNode c2;

        private BomDetail replacedDetail;

        [SetUp]
        public void SetUp()
        {
            this.c1 = new BomTreeNode
                          {
                              Type = "C",
                              Qty = 2,
                              Name = "CAP OLD",
                              HasChanged = true,
                              ReplacedBy = "CAP NEW",
                              Id = "4567"
                          };

            this.c2 = new BomTreeNode
                          {
                              Type = "C",
                              Qty = 2,
                              Name = "CAP NEW",
                              HasChanged = true,
                              ReplacementFor = "CAP OLD"
                          };

            this.newTree = new BomTreeNode
                               {
                                   Name = "BOM",
                                   Qty = 1,
                                   Type = "A",
                                   HasChanged = true,
                                   Children = new List<BomTreeNode> { this.c1, this.c2 }
                               };
            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(new Bom
                {
                    BomId = 100,
                    BomName = "BOM",
                    Details = new List<BomDetailViewEntry>
                                  {
                                        new BomDetailViewEntry
                                            {
                                                PartNumber = "CAP OLD",
                                                Qty = 2,
                                                ChangeState = "LIVE"
                                            }
                                  }
                });
            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(6666);
            this.DatabaseService.GetIdSequence("BOMDET_SEQ").Returns(10023);

            this.replacedDetail = new BomDetail { PartNumber = "CAP OLD", Qty = 2, ChangeState = "LIVE" };

            this.BomDetailRepository.FindById(4567)
                .Returns(this.replacedDetail);

            this.Sut.CreateBomChanges(this.newTree, 100, 33087);
        }

        [Test]
        public void ShouldAddBomChange()
        {
            this.BomChangeRepository
                .Received(1).Add(Arg.Any<BomChange>());
            this.BomChangeRepository
                .Received(1).Add(Arg.Is<BomChange>(c => c.BomName == "BOM" && c.DocumentNumber == 100));
        }

        [Test]
        public void ShouldAddReplacementDetail()
        {
            this.BomDetailRepository.Received(1).Add(Arg.Any<BomDetail>());
            this.BomDetailRepository.Received(1).Add(Arg.Is<BomDetail>(
                x => x.PartNumber == this.c2.Name
                     && x.Qty == this.c2.Qty
                     && x.ChangeState == "PROPOS"
                     && x.AddChangeId == 6666
                     && x.AddReplaceSeq == 1
                     && x.DetailId == 10023
                     && !x.DeleteChangeId.HasValue
                     && !x.DeleteReplaceSeq.HasValue
                     && x.BomId == 100));
        }

        [Test]
        public void ShouldUpdateReplacedDetail()
        {
            this.replacedDetail.DeleteReplaceSeq.Should().Be(1);
            this.replacedDetail.DeleteChangeId.Should().Be(6666);
            this.replacedDetail.ChangeState.Should().Be("PROPOS");
        }
    }
}
