namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenReplacingWithPartWithNoBomType : ContextBase
    {
        private BomTreeNode newTree;

        private BomTreeNode c1;

        private BomTreeNode c2;

        private BomDetail replacedDetail;

        private Action action;

        [SetUp]
        public void SetUp()
        {
            this.c1 = new BomTreeNode
            {
                Type = "C",
                Qty = 2,
                Name = "CAP OLD",
                AssemblyHasChanges = true,
                ReplacedBy = "CAP NEW",
                Id = "4567",
                ParentName = "BOM"
            };

            this.c2 = new BomTreeNode
            {
                Type = "C",
                Qty = 2,
                Name = "CAP NEW",
                AssemblyHasChanges = true,
                ReplacementFor = "4567",
                ParentName = "BOM"
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
                                                PartNumber = "CAP OLD",
                                                Qty = 2,
                                                ChangeState = "LIVE",
                                                DetailId = 4567
                                            }
                                  }
            });
            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(6666);
            this.DatabaseService.GetIdSequence("BOMDET_SEQ").Returns(10023);

            this.replacedDetail = new BomDetail
                                      {
                                          PartNumber = "CAP OLD", 
                                          Qty = 2, 
                                          ChangeState = "LIVE",
                                          AddChange = new BomChange { DocumentNumber = 999 }
                                      };
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { DecrementRule = "YES" });

            this.BomDetailRepository.FindById(4567)
                .Returns(this.replacedDetail);

            this.action = () => this.Sut.ProcessTreeUpdate(this.newTree, 100, 33087);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<InvalidBomChangeException>()
                .WithMessage($"Can't add CAP NEW to BOM - part has no BOM Type!");
        }
    }
}
