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

        private readonly IQueryRepository<WhatsInInspectionPurchaseOrdersData> whatsInInspectionPurchaseOrdersDataRepository;

        private readonly IQueryRepository<WhatsInInspectionStockLocationsData> whatsInInspectionStockLocationsDataRepository;

        private readonly IReportingHelper reportingHelper;

        public WhatsInInspectionReportService(
            IWhatsInInspectionRepository whatsInInspectionRepository,
            IQueryRepository<WhatsInInspectionPurchaseOrdersData> whatsInInspectionPurchaseOrdersDataRepository,
            IQueryRepository<WhatsInInspectionStockLocationsData> whatsInInspectionStockLocationsDataRepository,
            IReportingHelper reportingHelper)
        {
            this.whatsInInspectionRepository = whatsInInspectionRepository;
            this.whatsInInspectionPurchaseOrdersDataRepository = whatsInInspectionPurchaseOrdersDataRepository;
            this.whatsInInspectionStockLocationsDataRepository = whatsInInspectionStockLocationsDataRepository;
            this.reportingHelper = reportingHelper;
        }

        public IEnumerable<PartsInInspectionReportEntry> GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true,
            bool showBackOrdered = true)
        {
            var parts = this.whatsInInspectionRepository.GetWhatsInInspection(includeFailedStock)
                .Where(m => m.MinDate.HasValue).OrderBy(m => m.MinDate).ToList();

            var orders = this.whatsInInspectionPurchaseOrdersDataRepository.FilterBy(d => d.State.Equals("QC"))
                .ToList();

            var locationsData = new List<WhatsInInspectionStockLocationsData>();

            if (showStockLocations)
            {
                if (includeFailedStock)
                {
                    locationsData = this.whatsInInspectionStockLocationsDataRepository
                        .FilterBy(d => d.State.Equals("QC") || d.State.Equals("FAIL")).ToList();
                }
                else
                {
                    locationsData = this.whatsInInspectionStockLocationsDataRepository
                        .FilterBy(d => d.State.Equals("QC")).ToList();
                }
            }

            if (includeFailedStock)
            {
                orders.AddRange(this.whatsInInspectionPurchaseOrdersDataRepository.FilterBy(d => d.State.Equals("FAIL")));
            }

            return parts.Select(p => new PartsInInspectionReportEntry
                                         {
                                             PartNumber = p.PartNumber,
                                             Description = p.Description, 
                                             MinDate = p.MinDate,
                                             OurUnitOfMeasure = p.OurUnitOfMeasure,
                                             QtyInStock = p.QtyInStock,
                                             QtyInInspection = p.QtyInInspection,
                                             OrdersBreakdown = this
                                                 .BuildPurchaseOrdersBreakdown(orders.Where(o => o.PartNumber == p.PartNumber)),
                                             LocationsBreakdown = showStockLocations ? 
                                                                      this.BuildLocationsBreakdown(locationsData.Where(o => o.PartNumber == p.PartNumber))
                                                                    : null
                                         });
        }

        private ResultsModel BuildPurchaseOrdersBreakdown(
            IEnumerable<WhatsInInspectionPurchaseOrdersData> models)
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
                        new AxisDetailsModel("Received", "Qty Received", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("Passed", "Qty Passed", GridDisplayType.Value) { DecimalPlaces = 2 },
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
                            ColumnId = "Received",
                            Value = model.QtyReceived
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "Passed",
                            Value = model.QtyPassed
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

        private ResultsModel BuildLocationsBreakdown(
            IEnumerable<WhatsInInspectionStockLocationsData> models)
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
                        new AxisDetailsModel("Location", "Location",  GridDisplayType.TextValue),
                        new AxisDetailsModel("State", "State",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Batch", "Batch",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.Value) { DecimalPlaces = 2 },
                    });
            var values = new List<CalculationValueModel>();

            foreach (var model in models)
            {
                var currentRowId = $"{model.PartNumber + model.Location + model.Batch}";
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Location",
                        TextDisplay = model.Location
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "State",
                        TextDisplay = model.State
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "Batch",
                            TextDisplay = model.Batch
                        });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Qty",
                        Value = model.Qty
                    });
            }

            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }
    }
}
