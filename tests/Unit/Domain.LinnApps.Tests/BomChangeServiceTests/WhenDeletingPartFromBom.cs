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

    public class WhenDeletingPartFromBom : ContextBase
    {
        private BomTreeNode newTree;

        private BomTreeNode c1;

        private BomDetail deletedDetail;

        private BomTreeNode result;

        [SetUp]
        public void SetUp()
        {
            this.c1 = new BomTreeNode
                                    {
                                        Type = "A",
                                        Qty = 2,
                                        Name = "ASS 1",
                                        ChangeState = "PROPOS",
                                        ParentName = "BOM",
                                        ToDelete = true,
                                        Id = "4567",
                                        DeleteChangeDocumentNumber = 100
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
                    BomId = 100,
                    BomName = "BOM",
                    Details = new List<BomDetailViewEntry>
                                  {
                                      new BomDetailViewEntry
                                          {
                                              PartNumber = "ASS 1",
                                              Qty = 2,
                                              ChangeState = "PROPOS"
                                          }
                                  }
                });
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "ASS 1" });

            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(6666);
            this.deletedDetail = new BomDetail
                                     {
                                         PartNumber = "ASS 1", 
                                         Qty = 2, ChangeState = "PROPOS",
                                         DetailId = 4567,
                                         AddChange = new BomChange { ChangeId = 123 }
                                     };
            this.BomDetailRepository.FindById(4567)
                .Returns(this.deletedDetail);

            this.result = this.Sut.ProcessTreeUpdate(this.newTree, 100, 33087);
        }

        [Test]
        public void ShouldUndoDelete()
        {
            this.deletedDetail.DeleteChangeId.Should().BeNull();
        }

        [Test]
        public void ShouldReturnTree()
        {
            this.result.Children.First().DeleteChangeDocumentNumber.Should().BeNull();
        }
    }
}
