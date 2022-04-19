namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class WhatsInInspectionReportService : IWhatsInInspectionReportService
    {
        private readonly IWhatsInInspectionRepository whatsInInspectionRepository;

        private readonly IQueryRepository<WhatsInInspectionPurchaseOrdersViewModel> whatsInInspectionPurchaseOrdersViewRepository;

        private readonly IReportingHelper reportingHelper;

        public WhatsInInspectionReportService(
            IWhatsInInspectionRepository whatsInInspectionRepository,
            IQueryRepository<WhatsInInspectionPurchaseOrdersViewModel> whatsInInspectionPurchaseOrdersViewRepository,
            IReportingHelper reportingHelper)
        {
            this.whatsInInspectionRepository = whatsInInspectionRepository;
            this.whatsInInspectionPurchaseOrdersViewRepository = whatsInInspectionPurchaseOrdersViewRepository;
            this.reportingHelper = reportingHelper;
        }

        public IEnumerable<WhatsInInspectionReportModel> GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true,
            bool showBackOrdered = true)
        {
            var states = new List<string> { "QC" };
            if (includeFailedStock)
            {
                states.Add("FAIL");
            }

            var parts = this.whatsInInspectionRepository.GetWhatsInInspection(includeFailedStock)
                .Where(m => m.MinDate.HasValue).OrderBy(m => m.MinDate).ToList();

            var orders = this.whatsInInspectionPurchaseOrdersViewRepository.FilterBy(d => states.Contains(d.State)).ToList();

            return parts.Select(p => new WhatsInInspectionReportModel
                                         {
                                             PartNumber = p.PartNumber,
                                             Description = p.Description, 
                                             MinDate = p.MinDate,
                                             OurUnitOfMeasure = p.OurUnitOfMeasure,
                                             QtyInStock = p.QtyInStock,
                                             QtyInInspection = p.QtyInInspection,
                                             OrdersBreakdown = this
                                                 .BuildPurchaseOrdersBreakdown(orders.Where(o => o.PartNumber == p.PartNumber))
                                         });
        }

        private ResultsModel BuildPurchaseOrdersBreakdown(
            IEnumerable<WhatsInInspectionPurchaseOrdersViewModel> models)
        {
            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Orders Breakdown");

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("OrderNumber", "Order",  GridDisplayType.TextValue),
                        new AxisDetailsModel("OrderType", "Type",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty Ordered", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("Cancelled", "Cancelled", GridDisplayType.TextValue)
                    });
            var values = new List<CalculationValueModel>();

            foreach (var model in models)
            {
                var currentRowId = $"{model.OrderNumber + model.PartNumber}";
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "OrderNumber",
                            TextDisplay = model.OrderNumber.ToString()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "OrderType",
                            TextDisplay = model.OrderType.ToString()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "Qty",
                            Value = model.Qty
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "Cancelled",
                            TextDisplay = model.Cancelled
                        });
            }

            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }
    }
}
