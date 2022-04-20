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
        private IEnumerable<PartsInInspectionReportEntry> result;

        private bool includePartsWithNoOrderNumber;

        private bool showStockLocations;

        private bool includeFailedStock;

        private bool includeFinishedGoods;

        private bool showBackOrdered;

        [SetUp]
        public void SetUp()
        {
            this.includePartsWithNoOrderNumber = true;
            this.showStockLocations = false;
            this.includeFailedStock = true;
            this.includeFinishedGoods = false;
            this.showBackOrdered = false;

            this.result = new List<PartsInInspectionReportEntry> 
                              { 
                                  new PartsInInspectionReportEntry
                                      {
                                          PartNumber = "PART",
                                          OrdersBreakdown = new ResultsModel()
                                      }
                              };

            this.MockDomainService.GetReport(
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
                + $"&showBackOrdered={this.showBackOrdered}",
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
                this.includePartsWithNoOrderNumber,
                this.showStockLocations,
                this.includeFailedStock,
                this.includeFinishedGoods,
                this.showBackOrdered);
        }

        [Test]
        public void ShouldReturnReport()
        {
            var resource = this.Response.DeserializeBody<IEnumerable<WhatsInInspectionReportResource>>();
            resource.First().PartNumber.Should().Be("PART");
        }
    }
}

