namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class BomReportsService : IBomReportsService
    {
        private readonly IBomDetailViewRepository bomDetailViewRepository;

        private readonly IReportingHelper reportingHelper;

        private readonly IQueryRepository<BomCostReportDetail> bomCostReportDetails;

        private readonly IBomTreeService bomTreeService;

        public BomReportsService(
            IBomDetailViewRepository bomDetailViewRepository, 
            IReportingHelper reportingHelper, 
            IBomTreeService bomTreeService,
            IQueryRepository<BomCostReportDetail> bomCostReportDetails)
        {
            this.bomDetailViewRepository = bomDetailViewRepository;
            this.reportingHelper = reportingHelper;
            this.bomTreeService = bomTreeService;
            this.bomCostReportDetails = bomCostReportDetails;
        }

        public ResultsModel GetPartsOnBomReport(string bomName)
        {
            var lines = this.bomDetailViewRepository.FindAll().Where(d => d.BomPartNumber == bomName)
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
                            TextDisplay = line.Components?.ToList()
                                .Aggregate(string.Empty, (current, next) => current + $"{next.CircuitRef}, ")
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

            var treeNodes = partsOnBom as BomTreeNode[] ?? partsOnBom.ToArray();

            var ids = treeNodes.Select(x => x.Id).ToList();

            var details = this.bomCostReportDetails.FilterBy(x => ids.Select(
                i => string.IsNullOrEmpty(i) ? 0 : int.Parse(i)).Contains(x.DetailId)).ToList();

            details = details.OrderBy(d => ids.IndexOf(d.DetailId.ToString())).ToList();

            if (!splitBySubAssembly)
            {
                details.ForEach(d => d.BomName = bomName);
            }

            var assemblyGroups = details.ToList().GroupBy(x => x.BomName).ToList();

            foreach (var group in assemblyGroups)
            {
                var reportResult = new BomCostReport
                                       {
                                           SubAssembly = group.Key
                                       };
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
                            new("TotalMaterial", "Total Material",  GridDisplayType.Value) { DecimalPlaces = 5 },
                        });

                var values = new List<CalculationValueModel>();

                foreach (var member in group)
                {
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = member.PartNumber,
                                ColumnId = "PartNumber",
                                TextDisplay = member.PartNumber
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = member.PartNumber,
                                ColumnId = "Desc",
                                TextDisplay = member.PartDescription
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = member.PartNumber,
                                ColumnId = "Type",
                                TextDisplay = member.BomType
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = member.PartNumber,
                                ColumnId = "PreferredSupplier",
                                TextDisplay = member.PreferredSupplier.ToString()
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = member.PartNumber,
                                ColumnId = "Qty",
                                Value = member.Qty.GetValueOrDefault()
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = member.PartNumber,
                                ColumnId = "StdPrice",
                                Value = member.StandardPrice.GetValueOrDefault()
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = member.PartNumber,
                                ColumnId = "MaterialPrice",
                                Value = member.MaterialPrice.GetValueOrDefault()
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = member.PartNumber,
                                ColumnId = "TotalMaterial",
                                Value = member.Qty.GetValueOrDefault() * member.MaterialPrice.GetValueOrDefault()
                            });
                }
                reportLayout.SetGridData(values);
                reportResult.Breakdown = reportLayout.GetResultsModel();
                reportResult.MaterialTotal = Math.Round(
                    group.Sum(
                        x => x.Qty.GetValueOrDefault() * x.MaterialPrice.GetValueOrDefault()), 5);
                reportResult.StandardTotal = Math.Round(
                    group.Sum(
                        x => x.Qty.GetValueOrDefault() *  x.StandardPrice.GetValueOrDefault()), 5);

                results.Add(reportResult);
            }

            return results;
        }
    }
}
