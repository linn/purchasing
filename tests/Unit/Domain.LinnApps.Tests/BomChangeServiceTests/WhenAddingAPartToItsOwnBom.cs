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

    public class WhenAddingAPartToItsOwnBom : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var tree = new BomTreeNode
                           {
                               Name = "BOM",
                               Qty = 1,
                               Type = "A",
                               AssemblyHasChanges = true,
                               Children = new List<BomTreeNode> { new BomTreeNode { Name = "BOM", ParentName = "BOM" } },
                           };
            
            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(
                new Bom
                    {
                        BomName = "BOM", 
                        BomId = 123, 
                        Details = new List<BomDetailViewEntry>()
                    });
            this.BomDetailRepository.FindById(Arg.Any<int>()).Returns(new BomDetail());
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(
                    new Part { DecrementRule = "YES", BomType = "B" },
                    new Part { DecrementRule = "YES", BomType = "C" });
            this.action = () => this.Sut.ProcessTreeUpdate(tree, 100, 33087);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<InvalidBomChangeException>()
                .WithMessage($"Can't add BOM to it's own BOM!");
        }
    }
}
