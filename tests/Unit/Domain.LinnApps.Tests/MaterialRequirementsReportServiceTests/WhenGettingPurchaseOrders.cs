namespace Linn.Purchasing.Domain.LinnApps.Tests.MaterialRequirementsReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingPurchaseOrders : ContextBase
    {
        private string jobRef;

        private IList<string> partNumbers;

        private IEnumerable<MrPurchaseOrderDetail> result;

        [SetUp]
        public void SetUp()
        {
            this.jobRef = "ABC";
            this.partNumbers = new List<string> { "P1", "P2" };
            this.MrPurchaseOrderDetailRepository.FilterBy(Arg.Any<Expression<Func<MrPurchaseOrderDetail, bool>>>())
                .Returns(new List<MrPurchaseOrderDetail>
                             {
                                 new MrPurchaseOrderDetail { OrderNumber = 1, PartNumber = "P1" },
                                 new MrPurchaseOrderDetail { OrderNumber = 2, PartNumber = "P2" }
                             }.AsQueryable());
            this.result = this.Sut.GetMaterialRequirementsOrders(this.jobRef, this.partNumbers);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.result.Should().HaveCount(2);
            this.result.First(a => a.OrderNumber == 1).PartNumber.Should().Be("P1");
            this.result.First(a => a.OrderNumber == 2).PartNumber.Should().Be("P2");
        }
    }
}
