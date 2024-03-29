﻿namespace Linn.Purchasing.Facade.Tests.PurchaseOrdersReportServiceTests
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

    public class WhenGettingSuppliersWithUnacknowledgedOrdersReport : ContextBase
    {
        private IResult<ReportReturnResource> result;

        private SuppliersWithUnacknowledgedOrdersRequestResource requestResource;

        [SetUp]
        public void SetUp()
        {
            this.requestResource = new SuppliersWithUnacknowledgedOrdersRequestResource
                                       {
                                           VendorManager = "A",
                                           Planner = 123,
                                           UseSupplierGroup = true
                                       };
            this.DomainService.GetSuppliersWithUnacknowledgedOrders(
                    this.requestResource.Planner,
                    this.requestResource.VendorManager,
                    this.requestResource.UseSupplierGroup)
                .Returns(new ResultsModel { ReportTitle = new NameModel("Title") });
            this.result = this.Sut.GetSuppliersWithUnacknowledgedOrdersReport(this.requestResource);
        }

        [Test]
        public void ShouldCallDomainService()
        {
            this.DomainService
                .Received().GetSuppliersWithUnacknowledgedOrders(
                    this.requestResource.Planner,
                    this.requestResource.VendorManager,
                    this.requestResource.UseSupplierGroup);
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            var dataResult = ((SuccessResult<ReportReturnResource>)this.result).Data;
            dataResult.ReportResults.First().title.displayString.Should().Be("Title");
        }
    }
}
