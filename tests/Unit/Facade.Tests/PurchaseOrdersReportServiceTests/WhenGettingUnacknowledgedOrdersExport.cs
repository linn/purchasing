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
        private Stream resultsStream;

        private UnacknowledgedOrdersRequestResource requestResource;

        private int? supplierId;

        private int? organisationId;

        [SetUp]
        public void SetUp()
        {
            this.supplierId = 123;
            this.organisationId = 456;
            this.requestResource = new UnacknowledgedOrdersRequestResource
                                       {
                                           SupplierId = this.supplierId, OrganisationId = this.organisationId
                                       };
            this.DomainService.GetUnacknowledgedOrders(this.supplierId, this.organisationId)
                .Returns(new ResultsModel { ReportTitle = new NameModel("Title") });
            this.resultsStream = this.Sut.GetUnacknowledgedOrdersReportExport(this.requestResource, new List<string>());
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService
                .Received().GetUnacknowledgedOrders(this.supplierId, this.organisationId);
        }

        [Test]
        public void ShouldReturnMemoryStream()
        {
            this.resultsStream.Should().BeOfType<MemoryStream>();
        }
    }
}
