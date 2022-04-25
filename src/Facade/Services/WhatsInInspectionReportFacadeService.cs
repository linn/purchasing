namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources;

    public class WhatsInInspectionReportFacadeService : IWhatsInInspectionReportFacadeService
    {
        private readonly IWhatsInInspectionReportService domainService;

        private readonly IBuilder<ResultsModel> resultsModelResourceBuilder;

        public WhatsInInspectionReportFacadeService(
            IWhatsInInspectionReportService domainService,
            IBuilder<ResultsModel> resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<WhatsInInspectionReportResource> GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true,
            bool showBackOrdered = true,
            bool showOrders = true)
        {
            var result = this.domainService.GetReport(
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
                                                                        ? (ReportReturnResource)this
                                                                            .resultsModelResourceBuilder.Build(m.OrdersBreakdown, null)
                                                                        : null,
                                                                    LocationsBreakdown = m.LocationsBreakdown != null 
                                                                        ? (ReportReturnResource)this
                                                                        .resultsModelResourceBuilder.Build(m.LocationsBreakdown, null) 
                                                                        : null
                                                                }),
                        BackOrderData = result.BackOrderData != null 
                                         ? (ReportReturnResource)this.resultsModelResourceBuilder.Build(result.BackOrderData, null)
                                         : null
                });
        }

        public IEnumerable<IEnumerable<string>> GetTopLevelExport(
            bool includePartsWithNoOrderNumber = false,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true)
        {
            return this.domainService.GetTopLevelReport(
                includePartsWithNoOrderNumber,
                includeFailedStock,
                includeFinishedGoods).ConvertToCsvList();
        }
    }
}
