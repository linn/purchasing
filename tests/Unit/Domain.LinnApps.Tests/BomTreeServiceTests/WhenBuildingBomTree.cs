namespace Linn.Purchasing.Domain.LinnApps.Tests.BomTreeServiceTests
{
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NUnit.Framework;

    public class WhenBuildingBomTree : ContextBase
    {
        private BomTreeNode result;

        [SetUp]
        public void SetUp()
        {
            this.result = this.Sut.BuildTree("root", null);
        }

        [Test]
        public void ShouldBuildTree()
        {
            /* 
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
            this.result.Children.Count().Should().Be(3);
            this.result.Children.ElementAt(0).Name.Should().Be("n1");
            this.result.Children.ElementAt(1).Name.Should().Be("n2");
            this.result.Children.ElementAt(2).Name.Should().Be("n3");

            var n1 = this.result.Children.ElementAt(0);
            n1.Children.Count().Should().Be(3);
            n1.Children.ElementAt(0).Name.Should().Be("n11");
            n1.Children.ElementAt(1).Name.Should().Be("n12");
            n1.Children.ElementAt(2).Name.Should().Be("n13");

            var n11 = n1.Children.ElementAt(0);
            n11.Children.Count().Should().Be(2);
            n11.Children.ElementAt(0).Name.Should().Be("n111");
            n11.Children.ElementAt(1).Name.Should().Be("n112");

            var n13 = n1.Children.ElementAt(2);
            n13.Children.Count().Should().Be(1);
            n13.Children.ElementAt(0).Name.Should().Be("n131");

            var n111 = n11.Children.ElementAt(0);
            n111.Children.Count().Should().Be(1);
            n111.Children.ElementAt(0).Name.Should().Be("n1111");

            var n112 = n11.Children.ElementAt(1);
            n112.Children.Count().Should().Be(1);
            n112.Children.ElementAt(0).Name.Should().Be("n1121");
        }
    }
}
