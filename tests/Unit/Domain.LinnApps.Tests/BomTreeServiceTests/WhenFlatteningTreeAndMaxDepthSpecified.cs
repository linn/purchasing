namespace Linn.Purchasing.Domain.LinnApps.Tests.BomTreeServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NUnit.Framework;

    public class WhenFlatteningTreeAndMaxDepthSpecified : ContextBase
    {
        private IEnumerable<BomTreeNode> result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.FlattenTree("root", 2);
        }

        [Test]
        public void ShouldFlattenTruncatedTree()
        {
            /* flatten in level order
                  __root__
                 /    \   \
               _n1_    n2  n3
              /  |  \        
            n11 n12 n13     // should only be two levels deep

       */
            this.result.Count().Should().Be(7); // so seven elements in the list
            this.result.ElementAt(0).Name.Should().Be("root");
            this.result.ElementAt(1).Name.Should().Be("n1");
            this.result.ElementAt(2).Name.Should().Be("n2");
            this.result.ElementAt(3).Name.Should().Be("n3");
            this.result.ElementAt(4).Name.Should().Be("n11");
            this.result.ElementAt(5).Name.Should().Be("n12");
            this.result.ElementAt(6).Name.Should().Be("n13");
        }
    }
}
