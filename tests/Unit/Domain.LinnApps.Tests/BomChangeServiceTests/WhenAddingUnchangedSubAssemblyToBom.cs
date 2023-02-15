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

    public class WhenAddingUnchangedSubAssemblyToBom : ContextBase
    {
        private BomTreeNode newTree;

        private BomTreeNode subAssembly;

        [SetUp]
        public void SetUp()
        {
            this.subAssembly = new BomTreeNode
                          {
                              Type = "A",
                              Qty = 2,
                              Name = "ASS 1",
                              ParentName = "BOM",
                              Children = new List<BomTreeNode> { new BomTreeNode { Id = "666" } }
                          };
            this.newTree = new BomTreeNode
            {
                Name = "BOM",
                Qty = 1,
                Type = "A",
                AssemblyHasChanges = true,
                Children = new List<BomTreeNode> { this.subAssembly }
            };

            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(
                new Bom
                {
                    BomName = "BOM",
                    BomId = 123,
                    Details = new List<BomDetailViewEntry>()
                },
                new Bom
                    {
                        BomName = "ASS 1",
                        Details = new List<BomDetailViewEntry>
                                      {
                                          new BomDetailViewEntry { DetailId = 666 }
                                      }
                    });
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "ASS 1", DecrementRule = "YES", BomType = "A" });
            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(1);
            this.DatabaseService.GetIdSequence("BOMDET_SEQ").Returns(1);
            this.Sut.ProcessTreeUpdate(this.newTree, 100, 33087);
        }

        [Test]
        public void ShouldAddOneBomChange()
        {
            this.BomChangeRepository
                .Received(1).Add(Arg.Any<BomChange>());
            this.BomChangeRepository
                .Received(1).Add(Arg.Is<BomChange>(c => c.BomName == "BOM" && c.DocumentNumber == 100));
        }

        [Test]
        public void ShouldNotAddNewBom()
        {
            this.BomRepository.DidNotReceive().Add(Arg.Any<Bom>());
        }

        [Test]
        public void ShouldAddOneBomDetail()
        {
            this.BomDetailRepository.Received(1).Add(Arg.Any<BomDetail>());
            this.BomDetailRepository.Received(1).Add(Arg.Is<BomDetail>(
                x => x.PartNumber == this.subAssembly.Name
                     && x.Qty == this.subAssembly.Qty
                     && x.ChangeState == "PROPOS"
                     && x.DetailId == 1
                     && !x.DeleteChangeId.HasValue
                     && x.BomId == 123
                     && x.AddChangeId == 1));
        }
    }
}
