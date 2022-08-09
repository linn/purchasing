namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System.Collections.Generic;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class ForecastOrdersReportService : IForecastOrdersReportService
    {
        private readonly IQueryRepository<WeeklyForecastPart> weeklyForecastPartRepository;

        public ForecastOrdersReportService(IQueryRepository<WeeklyForecastPart> weeklyForecastPartRepository)
        {
            this.weeklyForecastPartRepository = weeklyForecastPartRepository;
        }

        public ResultsModel GetWeeklyExport(int supplierId)
        {
            var parts = this.weeklyForecastPartRepository.FilterBy(x => x.PreferredSupplier == supplierId);

            var reportLayout = new SimpleGridLayout(
                new ReportingHelper(),
                CalculationValueModelType.Value,
                null,
                null);

            var values = new List<CalculationValueModel>();

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("PartNumber", "Part Number",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Designation", "Designation",  GridDisplayType.TextValue),
                        new AxisDetailsModel("StartingQty", "Starting Qty", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("UnitPrice", "Unit Price", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("MinimumOrderQty", "MOQ",  GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("TotalNettReqtValue", "Qty In Inspection", GridDisplayType.Value) { DecimalPlaces = 2 },
                    });

            foreach (var part in parts)
            {
                var rowId = $"{part.MrPartNumber}";
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "PartNumber",
                            TextDisplay = part.MrPartNumber
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Designation",
                            TextDisplay = part.SupplierDesignation
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "StartingQty",
                            Value = part.StartingQty
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            Value = part.UnitPrice
                        });
                values.Add(
                    new CalculationValueModel 
                        {
                            RowId = rowId,
                            ColumnId = "MinimumOrderQty",
                            Value = part.MinimumOrderQty
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "TotalNettReqtValue",
                            Value = part.TotalNettReqtValue
                        });
            }

            reportLayout.SetGridData(values);

            reportLayout.ReportTitle = $"MR Parts for : {supplierId}";
            var model = reportLayout.GetResultsModel();

            return model;
        }
    }
}
