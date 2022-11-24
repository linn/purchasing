namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class BomReportsService : IBomReportsService
    {
        private readonly IBomDetailRepository bomDetailRepository;

        private readonly IReportingHelper reportingHelper;

        private readonly IQueryRepository<BoardComponentSummary> componentSummaryRepository;

        private readonly IBomTreeService bomTreeService;

        public BomReportsService(
            IBomDetailRepository bomDetailRepository, 
            IReportingHelper reportingHelper, 
            IQueryRepository<BoardComponentSummary> componentSummaryRepository,
            IBomTreeService bomTreeService)
        {
            this.bomDetailRepository = bomDetailRepository;
            this.reportingHelper = reportingHelper;
            this.componentSummaryRepository = componentSummaryRepository;
            this.bomTreeService = bomTreeService;
        }

        public ResultsModel GetPartsOnBomReport(string bomName)
        {
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
                            TextDisplay = this.componentSummaryRepository
                                .FilterBy(x => x.BomPartNumber == bomName && x.PartNumber == line.PartNumber)?.ToList()
                                .Aggregate(string.Empty, (current, next) => current + $"{next.Cref}, ")
                        });
            }
            reportLayout.AddValueDrillDownDetails(
                "Mr",
                $"/purchasing/boms/reports/list?bomName={{rowId}}",
                null,
                2,
                false);
            reportLayout.ReportTitle = bomName;
            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }

        public IEnumerable<BomCostReport> GetBomCostReport(
            string bomName, 
            bool splitBySubAssembly, 
            int levels, 
            decimal labourHourlyRate)
        {
            var results = new List<BomCostReport>();

            var partsOnBom = this.bomTreeService.FlattenBomTree(bomName, levels);

            if (!splitBySubAssembly)
            {
                foreach (var item in partsOnBom)
                {
                    item.ParentName = bomName;
                }
            }

            var assemblyGroups = partsOnBom.GroupBy(x => x.ParentName);

            foreach (var bomTreeNodes in assemblyGroups)
            {
                var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

                reportLayout.AddColumnComponent(
                    null,
                    new List<AxisDetailsModel>
                        {
                            new("PartNumber", "PartNumber", GridDisplayType.TextValue),
                            new("Desc", "Desc", GridDisplayType.TextValue),
                            new("Type", "Type", GridDisplayType.TextValue),
                            new("PreferredSupplier", GridDisplayType.TextValue),
                            new("Qty", "Qty", GridDisplayType.Value) { DecimalPlaces = 4 },
                            new("StdPrice", "Std Price", GridDisplayType.Value) { DecimalPlaces = 5 },
                            new("MaterialPrice", "Mat Price",  GridDisplayType.Value) { DecimalPlaces = 5 },
                            new("CPA", "CPA",  GridDisplayType.Value) { DecimalPlaces = 5 },
                            new("LabourTime", "Labour (mins)",  GridDisplayType.Value) { DecimalPlaces = 5 },
                            new("TotalMaterial", "Total Material",  GridDisplayType.Value) { DecimalPlaces = 5 },
                            new("TotalLabour", "Total Labour",  GridDisplayType.Value) { DecimalPlaces = 5 }
                        });
            }

            return results;
        }
    }
}
