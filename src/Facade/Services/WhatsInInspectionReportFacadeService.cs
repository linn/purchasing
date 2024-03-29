﻿namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources;

    public class WhatsInInspectionReportFacadeService : IWhatsInInspectionReportFacadeService
    {
        private readonly IWhatsInInspectionReportService domainService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public WhatsInInspectionReportFacadeService(
            IWhatsInInspectionReportService domainService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<WhatsInInspectionReportResource> GetReport(
            bool showGoodStockQty = false,
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true,
            bool showBackOrdered = true,
            bool showOrders = true)
        {
            var result = this.domainService.GetReport(
                    showGoodStockQty,
                    includePartsWithNoOrderNumber,
                    showStockLocations,
                    includeFailedStock,
                    includeFinishedGoods,
                    showBackOrdered,
                    showOrders);

            return new SuccessResult<WhatsInInspectionReportResource>(
                new WhatsInInspectionReportResource
                    {
                        PartsInInspection = result.PartsInInspection.Select(m => new WhatsInInspectionReportEntryResource
                                                                {
                                                                    PartNumber = m.PartNumber,
                                                                    Description = m.Description,
                                                                    QtyInStock = m.QtyInStock,
                                                                    QtyInInspection = m.QtyInInspection,
                                                                    OurUnitOfMeasure = m.OurUnitOfMeasure,
                                                                    Batch = m.Batch,
                                                                    OrdersBreakdown = m.OrdersBreakdown != null
                                                                        ? this.resultsModelResourceBuilder.Build(m.OrdersBreakdown)
                                                                        : null,
                                                                    LocationsBreakdown = m.LocationsBreakdown != null 
                                                                        ? this.resultsModelResourceBuilder.Build(m.LocationsBreakdown)
                                                                        : null
                                                                }),
                        BackOrderData = result.BackOrderData != null 
                                         ? this.resultsModelResourceBuilder.Build(result.BackOrderData)
                                         : null
                });
        }

        public IResult<IEnumerable<IEnumerable<string>>> GetTopLevelExport(
            bool showGoodStockQty = false,
            bool includePartsWithNoOrderNumber = false,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true)
        {
            return new SuccessResult<IEnumerable<IEnumerable<string>>>(this.domainService.GetTopLevelReport(
                showGoodStockQty,
                includePartsWithNoOrderNumber,
                includeFailedStock,
                includeFinishedGoods).ConvertToCsvList());
        }
    }
}
