namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingToBom : ContextBase
    {
        private BomTreeNode newTree;

        private BomTreeNode c11;

        private BomTreeNode c12;

        private BomTreeNode c1;

        private BomTreeNode result;

        [SetUp]
        public void SetUp()
        {
            this.c11 = new BomTreeNode
                          {
                                Type = "C",
                                Qty = 2,
                                Name = "COMP 11",
                                ParentName = "BOM"
                          };
            this.c12 = new BomTreeNode
                          {
                              Type = "C",
                              Qty = 2,
                              Name = "COMP 12",
                              ParentName = "BOM"
            };
            this.c1 = new BomTreeNode
                         {
                             Type = "A",
                             Qty = 2,
                             Name = "ASS 1",
                             ParentName = "BOM",
                             HasChanged = true,
                             Children = new List<BomTreeNode> { this.c11, this.c12 }
            };
            this.newTree = new BomTreeNode
                               {
                                   Name = "BOM",
                                   Qty = 1,
                                   Type = "A",
                                   HasChanged = true,
                                   Children = new List<BomTreeNode> { this.c1 }
                               };

            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(new Bom
                {
                    BomName = "BOM",
                    Details = new List<BomDetailViewEntry>()
                });

            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(1, 2);
            this.DatabaseService.GetIdSequence("BOMDET_SEQ").Returns(1, 2, 3);
            this.result = this.Sut.CreateBomChanges(this.newTree, 100, 33087);
        }

        [Test]
        public void ShouldAddTwoBomChanges()  // since we are updating the main bom and one subassembly on that bom
        {
           this.BomChangeRepository.Received(2).Add(Arg.Any<BomChange>());
        }

        [Test]
        public void ShouldAddThreeBomDetails()  // since we are adding one sub assembly with two components to the bom
        {
            this.BomDetailRepository.Received(3).Add(Arg.Any<BomDetail>());
        }
    }
}
