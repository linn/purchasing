namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using NSubstitute;

    using NUnit.Framework;

    public class WhenBomChangeForInvalidPart : ContextBase
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
                               HasChanged = true,
                               Children = new List<BomTreeNode> { new BomTreeNode { Name = "CAP 001", ParentName = "BOM" } }
                           };

            this.BomRepository.FindBy(Arg.Any<Expression<Func<Bom, bool>>>()).Returns(
                new Bom { BomName = "BOM", BomId = 123, Details = new List<BomDetailViewEntry>() });
            this.action = () => this.Sut.CreateBomChanges(tree, 100, 33087);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<ItemNotFoundException>()
                .WithMessage("Invalid Part Number: CAP 001 on Assembly: BOM");
        }
    }
}
