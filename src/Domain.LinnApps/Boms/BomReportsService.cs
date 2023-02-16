namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    public class BomReportsService : IBomReportsService
    {
        private readonly IBomDetailViewRepository bomDetailViewRepository;

        private readonly IReportingHelper reportingHelper;

        private readonly IQueryRepository<BomCostReportDetail> bomCostReportDetails;

        private readonly IRepository<CircuitBoard, string> boardRepository;

        private readonly IBomTreeService bomTreeService;

        public BomReportsService(
            IBomDetailViewRepository bomDetailViewRepository, 
            IReportingHelper reportingHelper, 
            IBomTreeService bomTreeService,
            IQueryRepository<BomCostReportDetail> bomCostReportDetails,
            IRepository<CircuitBoard, string> boardRepository)
        {
            this.bomDetailViewRepository = bomDetailViewRepository;
            this.reportingHelper = reportingHelper;
            this.bomTreeService = bomTreeService;
            this.bomCostReportDetails = bomCostReportDetails;
            this.boardRepository = boardRepository;
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

        public ResultsModel GetBoardDifferenceReport(
            string boardCode1,
            string revisionCode1,
            string boardCode2,
            string revisionCode2)
        {
            var board1 = this.boardRepository.FindById(boardCode1);
            var board2 = boardCode1 == boardCode2 ? board1 : this.boardRepository.FindById(boardCode2);

            if (board1 == null || board2 == null)
            {
                throw new ItemNotFoundException($"{board1} or {board2} could not be found");
            }

            var revision1 = board1.Layouts.SelectMany(a => a.Revisions).First(a => a.RevisionCode == revisionCode1);
            var revision2 = board2.Layouts.SelectMany(a => a.Revisions).First(a => a.RevisionCode == revisionCode2);
            var components1 = board1.ComponentsOnRevision(revision1.LayoutSequence, revision1.VersionNumber);
            var components2 = board2.ComponentsOnRevision(revision2.LayoutSequence, revision2.VersionNumber);

            var results = new ResultsModel
                              {
                                  ReportTitle = new NameModel($"Board differences between {boardCode1} {revisionCode1} and {boardCode2} {revisionCode2}")
                              };
            results.AddSortedColumns(new List<AxisDetailsModel>
                                         {
                                             new AxisDetailsModel("partNumber1", "Part Number", GridDisplayType.TextValue),
                                             new AxisDetailsModel("assemblyTech1", "As Tech", GridDisplayType.TextValue),
                                             new AxisDetailsModel("qty1", "Qty", GridDisplayType.TextValue),
                                             new AxisDetailsModel("partNumber2", "Part Number", GridDisplayType.TextValue),
                                             new AxisDetailsModel("assemblyTech2", "As Tech", GridDisplayType.TextValue),
                                             new AxisDetailsModel("qty2", "Qty", GridDisplayType.TextValue)
                                         });
            var valueModels = new List<CalculationValueModel>();

            var removed = components1.ExceptBy(components2.Select(a => a.CRef), component => component.CRef);
            foreach (var boardComponent in removed)
            {
                valueModels.Add(new CalculationValueModel
                               {
                                   RowId = boardComponent.CRef,
                                   ColumnId = "partNumber1",
                                   TextDisplay = boardComponent.PartNumber
                               });
                valueModels.Add(new CalculationValueModel
                               {
                                   RowId = boardComponent.CRef,
                                   ColumnId = "assemblyTech1",
                                   TextDisplay = boardComponent.AssemblyTechnology
                               });
                valueModels.Add(new CalculationValueModel
                               {
                                   RowId = boardComponent.CRef,
                                   ColumnId = "qty1",
                                   TextDisplay = boardComponent.Quantity.ToString("F1")
                               });
            }

            var added = components2.ExceptBy(components1.Select(a => a.CRef), component => component.CRef);
            foreach (var boardComponent in added)
            {
                valueModels.Add(new CalculationValueModel
                                    {
                                        RowId = boardComponent.CRef,
                                        ColumnId = "partNumber2",
                                        TextDisplay = boardComponent.PartNumber
                                    });
                valueModels.Add(new CalculationValueModel
                                    {
                                        RowId = boardComponent.CRef,
                                        ColumnId = "assemblyTech2",
                                        TextDisplay = boardComponent.AssemblyTechnology
                                    });
                valueModels.Add(new CalculationValueModel
                                    {
                                        RowId = boardComponent.CRef,
                                        ColumnId = "qty2",
                                        TextDisplay = boardComponent.Quantity.ToString("F1")
                                    });
            }

            var combined = components1.Concat(components2);
            var crefGroups = combined.GroupBy(a => a.CRef);

            foreach (var crefGroup in crefGroups)
            {
                if (crefGroup.Count() == 2 && crefGroup.First().PartNumber != crefGroup.Last().PartNumber)
                {
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "partNumber1",
                                       TextDisplay = crefGroup.First().PartNumber
                    });
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "assemblyTech1",
                                       TextDisplay = crefGroup.First().AssemblyTechnology
                                   });
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "qty1",
                                       TextDisplay = crefGroup.First().Quantity.ToString("F1")
                                   });
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "partNumber2",
                                       TextDisplay = crefGroup.Last().PartNumber
                                   });
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "assemblyTech2",
                                       TextDisplay = crefGroup.Last().AssemblyTechnology
                                   });
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "qty2",
                                       TextDisplay = crefGroup.Last().Quantity.ToString("F1")
                                   });
                }
            }
            
            this.reportingHelper.AddResultsToModel(
                results,
                valueModels.OrderBy(a => a.RowId).ToList(),
                CalculationValueModelType.TextValue,
                true);

            return results;
        }
    }
}
