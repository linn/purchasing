namespace Linn.Purchasing.Domain.LinnApps.Tests.BomTreeServiceTests
{
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NUnit.Framework;

    public class WhenBuildingBomTreeAndMaxDepthSpecified : BomContextBase
    {
        private BomTreeNode result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.BuildBomTree("root", 1, false);
        }

        [Test]
        public void ShouldBuildTruncatedTree()
        {
            /* 
                   __root__
                  /    \   \
                n1    n2    n3  // should only be one level deep      
            
        */
            this.result.Children.Count().Should().Be(3);
            Assert.That(this.result.Children.All(c => c.Children == null));
        }
    }
}
