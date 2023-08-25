namespace Linn.Purchasing.Domain.LinnApps.Tests.BomChangeServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAddingToAComponent : ContextBase
    {
        private Action action;

        [SetUp]
        public void SetUp()
        {
            var tree = new BomTreeNode
                           {
                               Name = "COMP",
                               Qty = 1,
                               Type = "A",
                               AssemblyHasChanges = true,
                               Children = new List<BomTreeNode> { new BomTreeNode { Name = "CAP 001", ParentName = "BOM" } }
                           };
            
            this.PartRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { PartNumber = "COMP", BomType = "C", DecrementRule = "YES" });
            this.action = () => this.Sut.ProcessTreeUpdate(tree, 100, 33087);
        }

        [Test]
        public void ShouldThrow()
        {
            this.action.Should().Throw<InvalidBomChangeException>()
                .WithMessage($"COMP is a Component! Can't add to its BOM");
        }
    }
}
