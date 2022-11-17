namespace Linn.Purchasing.Domain.LinnApps.Tests.BomTreeServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NUnit.Framework;

    public class WhenFlatteningTree : ContextBase
    {
        private IEnumerable<BomTreeNode> result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.FlattenBomTree("root", null, false);
        }

        [Test]
        public void ShouldFlattenToAListPreservingLevelOrder()
        {
            /* flatten in level order
                  __root__
                 /    \   \
               _n1_    n2  n3
              /  |  \        
            n11 n12 n13     
           /   \      \
         n111   n112   n131
         /       |
       n1111      n1121

       */
            this.result.Count().Should().Be(12);
            this.result.ElementAt(0).Name.Should().Be("root"); 
            this.result.ElementAt(1).Name.Should().Be("n1");
            this.result.ElementAt(2).Name.Should().Be("n2");
            this.result.ElementAt(3).Name.Should().Be("n3");
            this.result.ElementAt(4).Name.Should().Be("n11");
            this.result.ElementAt(5).Name.Should().Be("n12");
            this.result.ElementAt(6).Name.Should().Be("n13");
            this.result.ElementAt(7).Name.Should().Be("n111");
            this.result.ElementAt(8).Name.Should().Be("n112");
            this.result.ElementAt(9).Name.Should().Be("n131");
            this.result.ElementAt(10).Name.Should().Be("n1111");
            this.result.ElementAt(11).Name.Should().Be("n1121");
        }
    }
}
