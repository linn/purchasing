namespace Linn.Purchasing.Facade.Tests.PurchaseOrdersReportServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingUnacknowledgedOrdersExport : ContextBase
    {
        private IResult<IEnumerable<IEnumerable<string>>> result;

        private UnacknowledgedOrdersRequestResource requestResource;

        private int? supplierId;

        private int? supplierGroupId;

        [SetUp]
        public void SetUp()
        {
            this.supplierId = 123;
            this.supplierGroupId = 456;
            this.requestResource = new UnacknowledgedOrdersRequestResource
                                       {
                                           SupplierId = this.supplierId, SupplierGroupId = this.supplierGroupId
                                       };
            this.DomainService.GetUnacknowledgedOrders(this.supplierId, this.supplierGroupId)
                .Returns(new ResultsModel { ReportTitle = new NameModel("Title") });
            this.result = this.Sut.GetUnacknowledgedOrdersReportExport(this.requestResource);
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService
                .Received().GetUnacknowledgedOrders(this.supplierId, this.supplierGroupId);
        }


        [Test]
        public void ShouldReturnOk()
        {
            this.result.Should().BeOfType<SuccessResult<IEnumerable<IEnumerable<string>>>>();
        }
    }
}
