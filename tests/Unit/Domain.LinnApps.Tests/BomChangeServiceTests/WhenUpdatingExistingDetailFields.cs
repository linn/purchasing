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

    public class WhenUpdatingExistingDetailFields : ContextBase
    {
        private BomDetail detail;

        [SetUp]
        public void SetUp()
        {
            var tree = new BomTreeNode
            {
                Name = "BOM",
                Qty = 2,
                Type = "A",
                HasChanged = true,
                Children = new List<BomTreeNode>
                                              {
                                                  new BomTreeNode
                                                    {
                                                        Name = "CAP 530", 
                                                        ParentName = "BOM", 
                                                        Id = "123", 
                                                        Qty = 2
                                                    }
                                              },
            };
            this.detail = new BomDetail
                              {
                                  DetailId = 123,
                                  Qty = 1,
                                  AddChangeId = 666,
                                  AddChange = new BomChange {DocumentNumber = 100}
                              };
            this.BomDetailRepository.FindById(123).Returns(this.detail);
            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(
                new Bom
                {
                    BomName = "BOM",
                    BomId = 123,
                    Details = new List<BomDetailViewEntry> { new BomDetailViewEntry { DetailId = 123, Qty = 1 } }
                });
            this.BomChangeRepository.FindBy(Arg.Any<Expression<Func<BomChange, bool>>>())
                .Returns(new BomChange { ChangeId = 666, DocumentNumber = 100 });
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { DecrementRule = "YES", BomType = "C" });
            this.Sut.ProcessTreeUpdate(tree, 100, 33087);
        }

        [Test]
        public void ShouldUpdateField()
        {
            this.detail.Qty.Should().Be(2);
        }
    }
}
