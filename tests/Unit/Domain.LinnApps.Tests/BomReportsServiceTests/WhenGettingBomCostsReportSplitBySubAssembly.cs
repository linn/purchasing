namespace Linn.Purchasing.Domain.LinnApps.Tests.BomReportsServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBomCostsReportSplitBySubAssembly : ContextBase
    {
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
                                  new BomTreeNode { Id = 7 }
                              };
            this.BomTreeService.FlattenBomTree("SK HUB", null, true, false).Returns(flatBom);
            var details = new List<BomCostReportDetail>
                              {
                                  new BomCostReportDetail { DetailId = 3 }
                              }.AsQueryable();

            this.BomCostReportDetailsRepository.FilterBy(Arg.Any<Expression<Func<BomCostReportDetail, bool>>>())
                .Returns(details);
        }
    }
}
