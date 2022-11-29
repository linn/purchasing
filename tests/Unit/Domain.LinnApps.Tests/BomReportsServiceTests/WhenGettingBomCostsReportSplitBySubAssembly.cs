namespace Linn.Purchasing.Domain.LinnApps.Tests.BomReportsServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBomCostsReportSplitBySubAssembly : ContextBase
    {
        private IEnumerable<BomCostReport> results;

        [SetUp]
        public void SetUp()
        {
            var flatBom = new List<BomTreeNode>
                              {
                                  new BomTreeNode { Id = 1 },
                                  new BomTreeNode { Id = 2 },
                                  new BomTreeNode { Id = 3 },
                                  new BomTreeNode { Id = 4 },
                                  new BomTreeNode { Id = 5 },
                                  new BomTreeNode { Id = 6 },
                                  new BomTreeNode { Id = 7 },
                                  new BomTreeNode { Id = 8 },
                                  new BomTreeNode { Id = 9 },
                                  new BomTreeNode { Id = 10 }
                              };
            this.BomTreeService.FlattenBomTree("SK HUB", 999).Returns(flatBom);
            var details = new List<BomCostReportDetail>
                              {
                                  new BomCostReportDetail { DetailId = 1, BomName = "SK HUB", PartNumber = "MCAS 073" },
                                  new BomCostReportDetail { DetailId = 2, BomName = "SK HUB", PartNumber = "CONN 493" },
                                  new BomCostReportDetail { DetailId = 3, BomName = "SK HUB", PartNumber = "SELEKT BITS" },
                                  new BomCostReportDetail { DetailId = 4, BomName = "SK HUB", PartNumber = "MCP 1000" },
                                  new BomCostReportDetail { DetailId = 5, BomName = "SK HUB", PartNumber = "PCAS 1234" },
                                  new BomCostReportDetail { DetailId = 6, BomName = "MCAS 073", PartNumber = "RES 516" },
                                  new BomCostReportDetail { DetailId = 7, BomName = "SELEKT BITS", PartNumber = "SELEKT DIAL" },
                                  new BomCostReportDetail { DetailId = 8, BomName = "SELEKT DIAL", PartNumber = "SCREW" },
                                  new BomCostReportDetail { DetailId = 9, BomName = "PRINTING BITS", PartNumber = "SELEKT BITS" },
                                  new BomCostReportDetail { DetailId = 10, BomName = "PRINTING BITS", PartNumber = "INK" },
                                
                              }.AsQueryable();
            this.BomCostReportDetailsRepository.FilterBy(Arg.Any<Expression<Func<BomCostReportDetail, bool>>>())
                .Returns(details);

            this.results = this.Sut.GetBomCostReport("SK HUB", true, 999, 15);
        }

        [Test]
        public void ShouldReturnReportGroupedBySubAssembly()
        {
            // five different assemblies: SK HUB, MCAS 073, SELEKT BITS, PRINTING BITS, SELEKT DIAL
            this.results.Count().Should().Be(5);
            this.results.ElementAt(0).Breakdown.Rows.Count().Should().Be(5); // five things on the SK HUB bom
            this.results.ElementAt(1).Breakdown.Rows.Count().Should().Be(1); // one thing on the MCAS 073 bom
            this.results.ElementAt(2).Breakdown.Rows.Count().Should().Be(1); // one thing on the SELEKT BITS bom
            this.results.ElementAt(3).Breakdown.Rows.Count().Should().Be(1); // one thing on the SELEKT DIAL bom
            this.results.ElementAt(4).Breakdown.Rows.Count().Should().Be(2); // two things on the PRINTING BITS bom
        }

        [Test]
        public void ShouldPreserveTreeOrder()
        {
            this.results.First().SubAssembly.Should().Be("SK HUB");
            this.results.ElementAt(1).SubAssembly.Should().Be("MCAS 073");
            this.results.ElementAt(2).SubAssembly.Should().Be("SELEKT BITS");
            this.results.ElementAt(3).SubAssembly.Should().Be("SELEKT DIAL");
            this.results.ElementAt(4).SubAssembly.Should().Be("PRINTING BITS");
        }
    }
}
