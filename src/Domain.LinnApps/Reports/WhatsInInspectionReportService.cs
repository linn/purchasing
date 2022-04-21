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

        private readonly IQueryRepository<WhatsInInspectionBackOrderData> whatsInInspectionBackOrderDataRepository;

        private readonly IReportingHelper reportingHelper;

        public WhatsInInspectionReportService(
            IWhatsInInspectionRepository whatsInInspectionRepository,
            IQueryRepository<WhatsInInspectionPurchaseOrdersData> whatsInInspectionPurchaseOrdersDataRepository,
            IQueryRepository<WhatsInInspectionStockLocationsData> whatsInInspectionStockLocationsDataRepository,
            IQueryRepository<WhatsInInspectionBackOrderData> whatsInInspectionBackOrderDataRepository,
            IReportingHelper reportingHelper)
        {
            this.whatsInInspectionRepository = whatsInInspectionRepository;
            this.whatsInInspectionPurchaseOrdersDataRepository = whatsInInspectionPurchaseOrdersDataRepository;
            this.whatsInInspectionStockLocationsDataRepository = whatsInInspectionStockLocationsDataRepository;
            this.whatsInInspectionBackOrderDataRepository = whatsInInspectionBackOrderDataRepository;
            this.reportingHelper = reportingHelper;
        }

        public WhatsInInspectionReport GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true,
            bool showBackOrdered = true)
        {
            var parts = this.whatsInInspectionRepository.GetWhatsInInspection(includeFailedStock)
                .Where(m => m.MinDate.HasValue).OrderBy(m => m.MinDate).ToList();

            if (!includeFinishedGoods)
            {
                parts = parts.Where(p => p.RawOrFinished.Equals("RM")).ToList();
            }

            var orders = this.whatsInInspectionPurchaseOrdersDataRepository.FilterBy(d => d.State.Equals("QC"))
                .ToList();

            IEnumerable<WhatsInInspectionStockLocationsData> locationsData = null;

            IEnumerable<WhatsInInspectionBackOrderData> backOrderData = null;

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

            if (!includePartsWithNoOrderNumber)
            {
                parts = parts.Where(p => orders.Select(o => o.PartNumber)
                    .Contains(p.PartNumber)).ToList();
            }

            var partsResult = parts.Select(p => new PartsInInspectionReportEntry
                                                    {
                                                        PartNumber = p.PartNumber,
                                                        Description = p.Description,
                                                        MinDate = p.MinDate,
                                                        OurUnitOfMeasure = p.OurUnitOfMeasure,
                                                        QtyInStock = p.QtyInStock,
                                                        QtyInInspection = p.QtyInInspection,
                                                        OrdersBreakdown = this
                                                            .BuildPurchaseOrdersBreakdown(orders
                                                                .Where(o => o.PartNumber == p.PartNumber)),
                                                        LocationsBreakdown = showStockLocations ?
                                                                                 this.BuildLocationsBreakdown(
                                                                                     locationsData.Where(o => o.PartNumber == p.PartNumber))
                                                                                 : null
                                                    });

            if (showBackOrdered)
            {
                backOrderData = this.whatsInInspectionBackOrderDataRepository
                    .FindAll().ToList().Where(d => partsResult
                    .Select(x => x.PartNumber).Contains(d.ArticleNumber));
            }

            return new WhatsInInspectionReport 
                       { 
                           PartsInInspection = partsResult, 
                           BackOrderData = showBackOrdered 
                                               ? this.BuildBackOrderResultsModel(backOrderData)
                                               : null
                       };
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
                            TextDisplay = model.OrderType
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

        private ResultsModel BuildBackOrderResultsModel(
            IEnumerable<WhatsInInspectionBackOrderData> models)
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
                        new AxisDetailsModel("ArticleNumber", "Part",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Story", "Story",  GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyInInspection", "In Inspection",  GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("QtyNeeded", "Needed", GridDisplayType.Value) { DecimalPlaces = 2 },
                    });
            var values = new List<CalculationValueModel>();

            foreach (var model in models)
            {
                var currentRowId = $"{model.ArticleNumber}";
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ArticleNumber",
                        TextDisplay = model.ArticleNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Story",
                        TextDisplay = model.Story
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "QtyInInspection",
                        Value = model.QtyInInspection
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "QtyNeeded",
                            Value = model.QtyNeeded
                        });
            }

            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }
    }
}
