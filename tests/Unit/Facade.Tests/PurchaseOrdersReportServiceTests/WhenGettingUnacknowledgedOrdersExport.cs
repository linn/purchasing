namespace Linn.Purchasing.Facade.Tests.PurchaseOrdersReportServiceTests
{
    using System.Collections.Generic;
    using System.IO;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingUnacknowledgedOrdersExport : ContextBase
    {
        private IEnumerable<IEnumerable<string>> csvData;

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
            this.csvData = this.Sut.GetUnacknowledgedOrdersReportExport(this.requestResource, new List<string>());
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService
                .Received().GetUnacknowledgedOrders(this.supplierId, this.supplierGroupId);
        }

        [Test]
        public void ShouldReturnMemoryStream()
        {
            this.csvData.Should().BeOfType<List<List<string>>>();
        }
    }
}
