namespace Linn.Purchasing.Facade.Tests.PurchaseOrdersReportServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources.RequestResources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingUnacknowledgedOrdersReport : ContextBase
    {
        private IResult<ReportReturnResource> result;

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
            this.result = this.Sut.GetUnacknowledgedOrdersReport(this.requestResource, new List<string>());
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService
                .Received().GetUnacknowledgedOrders(this.supplierId, this.organisationId);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            var dataResult = ((SuccessResult<ReportReturnResource>)this.result).Data;
            dataResult.ReportResults.First().title.displayString.Should().Be("Title");
        }
    }
}
