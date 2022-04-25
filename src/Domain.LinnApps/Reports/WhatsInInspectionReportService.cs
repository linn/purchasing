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
            bool showBackOrdered = true,
            bool showOrders = true)
        {
            var parts = this.whatsInInspectionRepository.GetWhatsInInspection(includeFailedStock)
                .Where(m => m.MinDate.HasValue).OrderBy(m => m.MinDate).ToList();

            if (!includeFinishedGoods)
            {
                parts = parts.Where(p => p.RawOrFinished.Equals("R")).ToList();
            }

            var orders = this.whatsInInspectionPurchaseOrdersDataRepository.FilterBy(d => d.State.Equals("QC"))
                .ToList();

            IEnumerable<WhatsInInspectionStockLocationsData> locationsData = null;

            IEnumerable<WhatsInInspectionBackOrderData> backOrderData = null;

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
                                                        Batch = locationsData.Where(x => x.PartNumber
                                                            .Equals(p.PartNumber)).OrderBy(l => l.StockRotationDate).First().Batch,
                                                        QtyInInspection = p.QtyInInspection,
                                                        OrdersBreakdown = showOrders ? this
                                                            .BuildPurchaseOrdersBreakdown(orders
                                                                .Where(o => o.PartNumber == p.PartNumber))
                                                            : null,
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

        public ResultsModel GetTopLevelReport(
            bool includePartsWithNoOrderNumber = false,
            bool includeFailedStock = false,
            bool includeFinishedGoods = true)
        {
            var data = this.GetReport(
                includePartsWithNoOrderNumber,
                true,
                includeFailedStock,
                includeFinishedGoods,
                false,
                false);

            IEnumerable<WhatsInInspectionStockLocationsData> locationsData = null;

            IEnumerable<WhatsInInspectionBackOrderData> backOrderData = null;

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

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Orders Breakdown");

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("PartNumber", "PartNumber",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Description", "Description",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Batch", "Batch", GridDisplayType.TextValue),
                        new AxisDetailsModel("Date", "Date", GridDisplayType.TextValue),
                        new AxisDetailsModel("Units", "Units",  GridDisplayType.TextValue),
                        new AxisDetailsModel("QtyInStock", "Qty In Stock", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("QtyInInspection", "Qty In Inspection", GridDisplayType.Value) { DecimalPlaces = 2 }
                    });

            var values = new List<CalculationValueModel>();

            foreach (var line in data.PartsInInspection)
            {
                var currentRowId = line.PartNumber;
                
                var locationLine = locationsData.Where(l => l.PartNumber.Equals(line.PartNumber))
                    .OrderBy(d => d.StockRotationDate).First();

                var date = locationLine.StockRotationDate;

                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "PartNumber",
                        TextDisplay = line.PartNumber
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Description",
                        TextDisplay = line.Description
                    });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Batch",
                        TextDisplay = locationLine.BatchRef
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "Date",
                            TextDisplay = date.HasValue ? $"{date?.Day}/{date?.Month}/{date?.Year}"
                                              : string.Empty
                        });
                values.Add(
                    new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Units",
                        TextDisplay = line.OurUnitOfMeasure
                    });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "QtyInStock",
                            Value = line.QtyInStock
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "QtyInInspection",
                            Value = line.QtyInInspection
                        });
            }

            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
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
                        new AxisDetailsModel("OrderType", "Type",  GridDisplayType.TextValue),
                        new AxisDetailsModel("OrderNumber", "Order",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Cancelled", "Cancelled", GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty Ordered", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("Received", "Qty Received", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("QtyInInsp", "Qty Of Order In Insp", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("Passed", "Qty Passed", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("Returned", "Qty Returned", GridDisplayType.Value) { DecimalPlaces = 2 }
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
                            ColumnId = "QtyInInsp",
                            Value = model.QtyReceived - model.QtyPassed - model.QtyReturned
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
                            ColumnId = "Returned",
                            Value = model.QtyReturned
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
