namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenReplacingMultiplePartsOnBom : ContextBase
    {
        private BomTreeNode newTree;

        private BomTreeNode c1;

        private BomTreeNode c2;

        private BomTreeNode c3;

        private BomTreeNode c4;

        private BomDetail replacedDetail1;

        private BomDetail replacedDetail2;

        [SetUp]
        public void SetUp()
        {
            this.c1 = new BomTreeNode
            {
                Type = "C",
                Qty = 2,
                Name = "CAP 1 OLD",
                AssemblyHasChanges = true,
                ReplacedBy = "CAP 1 NEW",
                Id = "4567"
            };

            this.c2 = new BomTreeNode
            {
                Type = "C",
                Qty = 2,
                Name = "CAP 1 NEW",
                AssemblyHasChanges = true,
                Id = "4566",
                ReplacementFor = "4567"
            };

            this.c3 = new BomTreeNode
                          {
                              Type = "C",
                              Qty = 2,
                              Name = "CAP 2 OLD",
                              AssemblyHasChanges = true,
                              ReplacedBy = "CAP 2 NEW",
                              Id = "4568"
                          };

            this.c4 = new BomTreeNode
                          {
                              Type = "C",
                              Qty = 2,
                              Name = "CAP 2 NEW",
                              AssemblyHasChanges = true,
                              Id = "4655",
                              ReplacementFor = "4568"
                          };

            this.newTree = new BomTreeNode
            {
                Name = "BOM",
                Qty = 1,
                Type = "A",
                AssemblyHasChanges = true,
                Children = new List<BomTreeNode> { this.c1, this.c2, this.c3, this.c4 }
            };
            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(new Bom
            {
                BomId = 100,
                BomName = "BOM",
                Details = new List<BomDetailViewEntry>
                                  {
                                        new BomDetailViewEntry
                                            {
                                                PartNumber = "CAP 1 OLD",
                                                Qty = 2,
                                                ChangeState = "LIVE",
                                                DetailId = 4567
                                            },
                                        new BomDetailViewEntry
                                            {
                                                PartNumber = "CAP 2 OLD",
                                                Qty = 2,
                                                ChangeState = "LIVE",
                                                DetailId = 4568
                                            }
                                  }
            });
            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(6666);
            this.DatabaseService.GetIdSequence("BOMDET_SEQ").Returns(10023, 10024);

            this.replacedDetail1 = new BomDetail
            {
                PartNumber = "CAP OLD",
                Qty = 2,
                ChangeState = "LIVE",
                AddChange = new BomChange { DocumentNumber = 999 }
            };
            this.replacedDetail2 = new BomDetail
                                       {
                                           PartNumber = "CAP OLD",
                                           Qty = 2,
                                           ChangeState = "LIVE",
                                           AddChange = new BomChange { DocumentNumber = 999 }
                                       };
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { DecrementRule = "YES", BomType = "A" });
            
            this.BomDetailRepository.FilterBy(Arg.Any<Expression<Func<BomDetail, bool>>>())
                .Returns(new List<BomDetail>().AsQueryable());
            this.BomDetailRepository.FindById(4567)
                .Returns(this.replacedDetail1);
            this.BomDetailRepository.FindById(4568)
                .Returns(this.replacedDetail2);

            this.Sut.ProcessTreeUpdate(this.newTree, 100, 33087);
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
        public void ShouldAddReplacementDetails()
        {
            this.BomDetailRepository.Received(2).Add(Arg.Any<BomDetail>());
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
            this.BomDetailRepository.Received(1).Add(Arg.Is<BomDetail>(
                x => x.PartNumber == this.c4.Name
                     && x.Qty == this.c4.Qty
                     && x.ChangeState == "PROPOS"
                     && x.AddChangeId == 6666
                     && x.AddReplaceSeq == 2
                     && x.DetailId == 10024
                     && !x.DeleteChangeId.HasValue
                     && !x.DeleteReplaceSeq.HasValue
                     && x.BomId == 100));
        }

        [Test]
        public void ShouldUpdateReplacedDetail()
        {
            this.replacedDetail1.DeleteReplaceSeq.Should().Be(1);
            this.replacedDetail1.DeleteChangeId.Should().Be(6666);
            this.replacedDetail1.ChangeState.Should().Be("LIVE");

            this.replacedDetail2.DeleteReplaceSeq.Should().Be(2);
            this.replacedDetail2.DeleteChangeId.Should().Be(6666);
            this.replacedDetail2.ChangeState.Should().Be("LIVE");
        }
    }
}

