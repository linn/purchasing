namespace Linn.Purchasing.Facade.Tests.ResourceBuilderTests.OrdersBySupplierReportServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReportInvalidDate : ContextBase
    {
        private readonly int supplierId = 71234;

        private IResult<ReportReturnResource> result;

        [SetUp]
        public void SetUp()
        {
            var resource = new OrdersBySupplierSearchResource
                               {
                                   From = "01-Jan-2021", To = "01-Potato-2021", SupplierId = this.supplierId
                               };
            this.DomainService.GetOrdersBySupplierReport(1.January(2021), 1.June(2021), this.supplierId).Returns(
                new ResultsModel
                    {
                        ReportTitle = new NameModel("Purchase Orders By Supplier - 71234: Dwight K Schrute")
                    });

            this.result = this.Sut.GetOrdersBySupplierReport(resource, new List<string>());
        }

        [Test]
        public void ShouldNotCallDomain()
        {
            this.DomainService.DidNotReceiveWithAnyArgs().GetOrdersBySupplierReport(
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<int>());
        }

        [Test]
        public void ShouldReturnBadRequestWithMessage()
        {
            this.result.Should().BeOfType<BadRequestResult<ReportReturnResource>>();
            var dataResult = (BadRequestResult<ReportReturnResource>) this.result;
            dataResult.Message.Should().Be("Invalid dates supplied to orders by supplier report");
        }
    }
}
