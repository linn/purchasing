namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUndoingAReplacementMadeOnThisChangeRequest : ContextBase
    {
        private BomTreeNode newTree;

        private BomTreeNode c1;

        private BomTreeNode c2;

        private BomDetail replacementToUndo;

        private BomDetail replacedToUndo;

        private BomChange bomChange;

        [SetUp]
        public void SetUp()
        {
            this.bomChange = new BomChange { ChangeId = 123, DocumentNumber = 48451 };
            this.c1 = new BomTreeNode
            {
                Name = "CAP 530",
                Description = "68PF,,,,,TH,,,,,,",
                Qty = 1,
                Type = "C",
                ParentName = "PCAS LEWIS3",
                ParentId = "root",
                Id = "700730",
                ChangeState = "PROPOS",
                AddChangeDocumentNumber = 48451,
                AddReplaceSeq = 1,
                ToDelete = true,
                Requirement = "Y"
            };
            this.c2 = new BomTreeNode
                          {
                              Name = "LBL 111",
                              Description = "BLACK STICK ON DOTS  ",
                              Qty = 1,
                              Type = "C",
                              ParentName = "PCAS LEWIS3",
                              ParentId = "root",
                              Id = "700670",
                              ChangeState = "PROPOS",
                              AddChangeDocumentNumber = 48450,
                              DeleteChangeDocumentNumber = 48451,
                              DeleteReplaceSeq = 1,
                              Requirement = "Y",
                              SafetyCritical = "N"
                          };
            this.newTree = new BomTreeNode
            {
                Name = "BOM",
                Qty = 1,
                Type = "A",
                AssemblyHasChanges = true,
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
                                              PartNumber = "CAP 530",
                                              Qty = 1,
                                              DetailId = 700730,
                                              ChangeState = "PROPOS",
                                          },
                                      new BomDetailViewEntry
                                          {
                                              PartNumber = "LBL 111",
                                              Qty = 1,
                                              DetailId = 700670,
                                              ChangeState = "PROPOS",
                                          }
                                  }
            });

            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "ASS 1",  });

            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(6666);
            this.replacementToUndo = new BomDetail
            {
                PartNumber = "LBL 111",
                Qty = 1,
                ChangeState = "LIVE",
                DetailId = 700670,
                AddChange = this.bomChange,
                AddReplaceSeq = 1,
                GenerateRequirement = "Y"
            };

            this.replacedToUndo = 
                new BomDetail
                    {
                        PartNumber = "CAP 530",
                        Qty = 1,
                        GenerateRequirement = "Y",
                        DeleteReplaceSeq = 1, 
                        DeleteChange = this.bomChange
                    };

            this.BomDetailRepository.FindById(700730)
                .Returns(this.replacementToUndo);
            this.BomDetailRepository.FindById(700670)
                .Returns(this.replacedToUndo);

            this.BomDetailRepository
                .FindBy(Arg.Any<Expression<Func<BomDetail, bool>>>()).Returns(this.replacedToUndo);

            this.Sut.ProcessTreeUpdate(this.newTree, 48451, 33879);
        }

        [Test]
        public void ShouldRemoveReplacement()
        {
            this.BomDetailRepository.Received().Remove(Arg.Is<BomDetail>(x => x.DetailId == 700670));
        }

        [Test]
        public void ShouldRevertReplacedDetail()
        {
            this.replacedToUndo.DeleteReplaceSeq.Should().BeNull();
            this.replacedToUndo.DeleteChangeId.Should().BeNull();
        }
    }
}
