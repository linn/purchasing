
namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenMakingChangesToMultipleBoms : ContextBase
    {
        private BomTreeNode newTree;

        private BomTreeNode c11;

        private BomTreeNode c12;

        private BomTreeNode c1;

        [SetUp]
        public void SetUp()
        {
            this.c11 = new BomTreeNode
            {
                Type = "C",
                Qty = 2,
                Name = "COMP 11",
                Requirement = "Y",
                ParentName = "ASS 1"
            };
            this.c12 = new BomTreeNode
            {
                Type = "C",
                Qty = 2,
                Name = "COMP 12",
                Requirement = "Y",
                ParentName = "BOM"
            };
            this.c1 = new BomTreeNode
            {
                Type = "A",
                Qty = 2,
                Name = "ASS 1",
                Id = "345",
                ParentName = "BOM",
                Requirement = "Y",
                HasChanged = true,
                Children = new List<BomTreeNode> { this.c11 }
            };
            this.newTree = new BomTreeNode
            {
                Name = "BOM",
                Qty = 1,
                Type = "A",
                Requirement = "Y",
                HasChanged = true,
                Children = new List<BomTreeNode> { this.c1, this.c12 }
            };

            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(
                new Bom
                {
                    BomName = "BOM",
                    BomId = 123,
                    Details = new List<BomDetailViewEntry>
                                  {
                                       new BomDetailViewEntry
                                           {
                                               PartNumber = "ASS 1",
                                               DetailId = 345
                                           } 
                                  }
                },
                new Bom 
                    {
                        BomName = "BOM",
                        BomId = 123,
                        Details = new List<BomDetailViewEntry>
                                      {
                                          new BomDetailViewEntry
                                              {
                                                  PartNumber = "ASS 1",
                                                  DetailId = 345
                                              }
                                      }
                    },
                new Bom
                {
                    BomName = "ASS 1",
                    BomId = 456,
                    Details = new List<BomDetailViewEntry>()
                });
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { DecrementRule = "YES", BomType = "C" });
            this.BomDetailRepository.FindById(345).Returns(new BomDetail { Qty = 2, GenerateRequirement = "Y" });
            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(1, 2);
            this.DatabaseService.GetIdSequence("BOMDET_SEQ").Returns(1, 2, 3);
            this.Sut.ProcessTreeUpdate(this.newTree, 100, 33087);
        }

        [Test]
        public void ShouldAddTwoBomChanges()  // since we are updating the main bom and one subassembly on that bom
        {
            this.BomChangeRepository
                .Received(2).Add(Arg.Any<BomChange>());
            this.BomChangeRepository
                .Received(1).Add(Arg.Is<BomChange>(c => c.BomName == "BOM" && c.DocumentNumber == 100));
            this.BomChangeRepository
                .Received(1).Add(Arg.Is<BomChange>(c => c.BomName == "ASS 1" && c.DocumentNumber == 100));
        }

        [Test]
        public void ShouldNotAddNewBom()
        {
            this.BomRepository.DidNotReceive().Add(Arg.Any<Bom>());
        }

        [Test]
        public void ShouldAddTwoBomDetails() 
        {
            this.BomDetailRepository.Received(2).Add(Arg.Any<BomDetail>());
            this.BomDetailRepository.Received(1).Add(Arg.Is<BomDetail>(
                x => x.PartNumber == this.c11.Name
                     && x.Qty == this.c11.Qty
                     && x.ChangeState == "PROPOS"
                     && x.DetailId == 2
                     && !x.DeleteChangeId.HasValue
                     && x.BomId == 456
                     && x.AddChangeId == 2));
            this.BomDetailRepository.Received(1).Add(Arg.Is<BomDetail>(
                x => x.PartNumber == this.c12.Name
                     && x.Qty == this.c12.Qty
                     && x.ChangeState == "PROPOS"
                     && x.DetailId == 1
                     && !x.DeleteChangeId.HasValue
                     && x.BomId == 123
                     && x.AddChangeId == 1));
        }
    }
}
