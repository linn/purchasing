namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class PartsReceivedReportService : IPartsReceivedReportService
    {
        private readonly IQueryRepository<PartsReceivedViewModel> partsReceivedView;

        private readonly IReportingHelper reportingHelper;

        public PartsReceivedReportService(
            IQueryRepository<PartsReceivedViewModel> partsReceivedView,
            IReportingHelper reportingHelper)
        {
            this.partsReceivedView = partsReceivedView;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetReport(
            string jobref,
            int? supplier,
            string fromDate,
            string toDate,
            string orderBy,
            bool includeNegativeValues = true)
        {
            var from = DateTime.Parse(fromDate).Date;
            var to = DateTime.Parse(toDate).Date.AddDays(1).AddTicks(-1);
            var data = this.partsReceivedView.FilterBy(
                x => x.JobRef == jobref
                     && (!supplier.HasValue || x.SupplierId == supplier)
                     && x.DateBooked >= from
                     && x.DateBooked <= to);

            if (!includeNegativeValues)
            {
                data = data.Where(x => x.Qty >= 0);
            }

            if (supplier.HasValue)
            {
                data = data.Where(x => x.SupplierId == supplier);
            }

            data = orderBy switch
                {
                    "PART" => data.OrderBy(x => x.PartNumber),
                    "SUPPLIER" => data.OrderBy(x => x.SupplierName),
                    "OVERSTOCK" => data.OrderBy(x => x.OverstockQty),
                    "MATERIAL PRICE" => data.OrderBy(x => x.MaterialPrice),
                    _ => data.OrderBy(x => x.DateBooked)
                };

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Parts Received between {from.ToShortDateString()} and {to.ToShortDateString()}");
            
            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("PartNumber", "Part", GridDisplayType.TextValue),
                        new AxisDetailsModel("OrderNumber", "Order Number", GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.Value) { DecimalPlaces = 0 },
                        new AxisDetailsModel("SupplierName", "Supplier", GridDisplayType.TextValue),
                        new AxisDetailsModel("DateBooked", "Date Booked", GridDisplayType.TextValue),
                        new AxisDetailsModel("OverstockQty", "Overstock", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("MaterialPrice", "Material Price", GridDisplayType.Value) { DecimalPlaces = 2 }
                    });

            var values = new List<CalculationValueModel>();
            foreach (var datum in data)
            {
                var currentRowId = $"{datum.PartNumber + datum.OrderNumber}";

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "PartNumber",
                            TextDisplay = datum.PartNumber
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "OrderNumber",
                            TextDisplay = datum.OrderNumber
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "Qty",
                            Value = datum.Qty
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "SupplierName",
                            TextDisplay = datum.SupplierName
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "DateBooked",
                            TextDisplay = datum.DateBooked.ToShortDateString()
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "OverstockQty",
                            Value = datum.OverstockQty ?? 0
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = currentRowId,
                            ColumnId = "MaterialPrice",
                            Value = datum.MaterialPrice ?? 0
                        });
            }
            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }
    }
}
