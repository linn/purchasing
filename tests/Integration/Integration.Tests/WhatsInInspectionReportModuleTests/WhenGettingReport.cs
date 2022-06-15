namespace Linn.Purchasing.Integration.Tests.WhatsInInspectionReportModuleTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private WhatsInInspectionReport result;

        private bool includePartsWithNoOrderNumber;

        private bool showStockLocations;

        private bool includeFailedStock;

        private bool includeFinishedGoods;

        private bool showBackOrdered;

        private bool showOrders;

        private bool showGoodStockQty;

        [SetUp]
        public void SetUp()
        {
            this.includePartsWithNoOrderNumber = true;
            this.showStockLocations = false;
            this.includeFailedStock = true;
            this.includeFinishedGoods = false;
            this.showBackOrdered = false;
            this.showOrders = true;
            this.showGoodStockQty = true;

            this.result = new WhatsInInspectionReport
                              {
                                  PartsInInspection = new List<PartsInInspectionReportEntry>
                                                          {
                                                              new PartsInInspectionReportEntry
                                                                  {
                                                                      PartNumber = "PART",
                                                                      OrdersBreakdown = new ResultsModel()
                                                                  }
                                                          },
                                BackOrderData = new ResultsModel()
                              };
        

            this.MockDomainService.GetReport(
                this.showGoodStockQty,
                this.includePartsWithNoOrderNumber,
                this.showStockLocations,
                this.includeFailedStock,
                this.includeFinishedGoods,
                this.showBackOrdered).Returns(this.result);

            this.Response = this.Client.Get(
                $"/purchasing/reports/whats-in-inspection/report?includePartsWithNoOrderNumber=" 
                + $"{this.includePartsWithNoOrderNumber}&showStockLocations={this.showStockLocations}"
                + $"&includeFailedStock={this.includeFailedStock}"
                + $"&includeFinishedGoods={this.includeFinishedGoods}" 
                + $"&showBackOrdered={this.showBackOrdered}"
                + $"&showOrders={this.showOrders}"
                + $"&showGoodStockQty={this.showGoodStockQty}",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldPassCorrectOptionsToDomainService()
        {
            this.MockDomainService.Received().GetReport(
                this.showGoodStockQty,
                this.includePartsWithNoOrderNumber,
                this.showStockLocations,
                this.includeFailedStock,
                this.includeFinishedGoods,
                this.showBackOrdered);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<WhatsInInspectionReportResource>();
            resource.PartsInInspection.First().PartNumber.Should().Be("PART");
        }
    }
}

