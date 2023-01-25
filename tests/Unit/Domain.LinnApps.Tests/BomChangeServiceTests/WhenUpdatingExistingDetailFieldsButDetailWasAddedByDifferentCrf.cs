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

    public class WhenUpdatingExistingDetailFieldsButDetailWasAddedByDifferentCrf : ContextBase
    {
        private Action action;

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
                                                        Name = "CAP 530", ParentName = "BOM", Id = "123", Type = "C"
                                                    }
                                              },
                           };
            this.BomDetailRepository.FindById(123).Returns(new BomDetail { DetailId = 123, Qty = 1, AddChangeId = 666 });
            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(
                new Bom
                    {
                        BomName = "BOM",
                        BomId = 123,
                        Details = new List<BomDetailViewEntry> { new BomDetailViewEntry { DetailId = 123, Qty = 1 } }
                    });
            this.DatabaseService.GetIdSequence("CHG_SEQ").Returns(111);
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { DecrementRule = "YES", BomType = "C" });
            this.action = () => this.Sut.CreateBomChanges(tree, 100, 33087);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<InvalidBomChangeException>()
                .WithMessage("Can't directly update details added by a different CRF - Replace them instead!");
        }
    }
}
