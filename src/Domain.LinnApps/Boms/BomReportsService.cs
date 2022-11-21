namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;

    public class BomReportsService : IBomReportsService
    {
        private readonly IRepository<BomDetail, int> bomDetailRepository;

        private readonly IReportingHelper reportingHelper;

        public BomReportsService(IRepository<BomDetail, int> bomDetailRepository, IReportingHelper reportingHelper)
        {
            this.bomDetailRepository = bomDetailRepository;
            this.reportingHelper = reportingHelper;
        }

        public ResultsModel GetPartsOnBomReport(string bomName)
        {
            var x = this.bomDetailRepository.FindAll();
            var lines = this.bomDetailRepository.FindAll().Where(d => d.BomPartNumber == bomName)
                .OrderBy(x => x.PartNumber);

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new("Qty", "Qty",  GridDisplayType.Value) { DecimalPlaces = 5 },
                        new("Units", "Units", GridDisplayType.TextValue),
                        new("PartNumber", "Part Number",  GridDisplayType.TextValue),
                        new("Description", "Description",  GridDisplayType.TextValue),
                        new("BomType", "Bom Type", GridDisplayType.TextValue),
                        new("GenerateReqt", "Reqt?", GridDisplayType.TextValue),
                        new("DecrementRule", "Decr?", GridDisplayType.TextValue),
                        new("DrawingRef", "Drawing Ref?", GridDisplayType.TextValue),
                        new("PcasLine", "PCAS?", GridDisplayType.TextValue),
                        new("Crefs", "Crefs", GridDisplayType.TextValue)
                    });

            var values = new List<CalculationValueModel>();

            foreach (var line in lines)
            {
                var rowId = line.PartNumber;
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Qty",
                            Value = line.Qty
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Units",
                            TextDisplay = line.Part.OurUnitOfMeasure
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "PartNumber",
                            TextDisplay = line.PartNumber
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Description",
                            TextDisplay = line.Part.Description
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "BomType",
                            TextDisplay = line.Part.BomType
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "GenerateReqt",
                            TextDisplay = line.GenerateRequirement
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "DecrementRule",
                            TextDisplay = line.Part.DecrementRule
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "DrawingRef",
                            TextDisplay = line.Part.DrawingReference
                        });
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Crefs",
                            TextDisplay = line.ComponentSummary.Aggregate(string.Empty, (current, next) => current + $"{next.Cref}, ")
                        });
            }
            reportLayout.ReportTitle = bomName;
            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }
    }
}
