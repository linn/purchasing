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
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class BomReportsService : IBomReportsService
    {
        private readonly IBomDetailViewRepository bomDetailViewRepository;

        private readonly IReportingHelper reportingHelper;

        private readonly IQueryRepository<BomCostReportDetail> bomCostReportDetails;

        private readonly IQueryRepository<BomDetailComponent> bomDetailComponents;

        private readonly IRepository<CircuitBoard, string> boardRepository;

        private readonly IBomTreeService bomTreeService;

        private readonly IQueryRepository<Part> partRepository;

        public BomReportsService(
            IBomDetailViewRepository bomDetailViewRepository, 
            IReportingHelper reportingHelper, 
            IBomTreeService bomTreeService,
            IQueryRepository<BomCostReportDetail> bomCostReportDetails,
            IRepository<CircuitBoard, string> boardRepository,
            IQueryRepository<Part> partRepository,
            IQueryRepository<BomDetailComponent> bomDetailComponents)
        {
            this.bomDetailViewRepository = bomDetailViewRepository;
            this.reportingHelper = reportingHelper;
            this.bomTreeService = bomTreeService;
            this.bomCostReportDetails = bomCostReportDetails;
            this.boardRepository = boardRepository;
            this.partRepository = partRepository;
            this.bomDetailComponents = bomDetailComponents;
        }

        public ResultsModel GetPartsOnBomReport(string bomName)
        {
            var lines = this.bomDetailViewRepository.FindAll().Where(d => d.BomPartNumber == bomName && d.ChangeState == "LIVE")
                .OrderBy(x => x.PartNumber);

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.Value) { DecimalPlaces = 5 },
                        new AxisDetailsModel("Units", "Units", GridDisplayType.TextValue),
                        new AxisDetailsModel("PartNumber", "Part Number",  GridDisplayType.TextValue),
                        new AxisDetailsModel("Description", "Description",  GridDisplayType.TextValue),
                        new AxisDetailsModel("BomType", "Bom Type", GridDisplayType.TextValue),
                        new AxisDetailsModel("GenerateReqt", "Reqt?", GridDisplayType.TextValue),
                        new AxisDetailsModel("DecrementRule", "Decr?", GridDisplayType.TextValue),
                        new AxisDetailsModel("DrawingRef", "Drawing Ref?", GridDisplayType.TextValue),
                        new AxisDetailsModel("PcasLine", "PCAS?", GridDisplayType.TextValue),
                        new AxisDetailsModel("Crefs", "Crefs", GridDisplayType.TextValue)
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

            var partsOnBom = this.bomTreeService.FlattenBomTree(bomName, levels, false);

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
                var reportLayout = new SimpleGridLayout(
                    this.reportingHelper, CalculationValueModelType.Value, null, null);

                reportLayout.AddColumnComponent(
                    null,
                    new List<AxisDetailsModel>
                        {
                            new AxisDetailsModel("PartNumber", "PartNumber", GridDisplayType.TextValue),
                            new AxisDetailsModel("Desc", "Desc", GridDisplayType.TextValue),
                            new AxisDetailsModel("Type", "Type", GridDisplayType.TextValue),
                            new AxisDetailsModel("PreferredSupplier", GridDisplayType.TextValue),
                            new AxisDetailsModel("Qty", "Qty", GridDisplayType.Value) { DecimalPlaces = 4 },
                            new AxisDetailsModel("StdPrice", "Std Price", GridDisplayType.Value) { DecimalPlaces = 5 },
                            new AxisDetailsModel("MaterialPrice", "Mat Price",  GridDisplayType.Value) { DecimalPlaces = 5 },
                            new AxisDetailsModel("TotalMaterial", "Total Material",  GridDisplayType.Value) { DecimalPlaces = 5 },
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
                var part = this.partRepository.FindBy(p => p.PartNumber == group.Key);
                reportResult.StandardTotal = part
                    .MaterialPrice.GetValueOrDefault();

                results.Add(reportResult);
            }

            return results;
        }

        public ResultsModel GetBoardDifferenceReport(
            string boardCode1,
            string revisionCode1,
            string boardCode2,
            string revisionCode2,
            bool liveOnly)
        {
            if (string.IsNullOrWhiteSpace(boardCode1) || string.IsNullOrWhiteSpace(revisionCode1)
                                                      || string.IsNullOrWhiteSpace(boardCode2)
                                                      || string.IsNullOrWhiteSpace(revisionCode2))
            {
                throw new InvalidOptionException("Board or revision not specified");
            }

            var board1 = this.boardRepository.FindById(boardCode1.ToUpper());
            var board2 = boardCode1.ToUpper() == boardCode2.ToUpper() ? board1 
                             : this.boardRepository.FindById(boardCode2.ToUpper());

            if (board1 == null || board2 == null)
            {
                throw new ItemNotFoundException($"{boardCode1.ToUpper()} or {boardCode2.ToUpper()} could not be found");
            }

            var revision1 = board1.Layouts.SelectMany(a => a.Revisions).First(a => a.RevisionCode == revisionCode1.ToUpper());
            var revision2 = board2.Layouts.SelectMany(a => a.Revisions).First(a => a.RevisionCode == revisionCode2.ToUpper());
            var components1 = board1.ComponentsOnRevision(revision1.LayoutSequence, revision1.VersionNumber, liveOnly);
            var components2 = board2.ComponentsOnRevision(revision2.LayoutSequence, revision2.VersionNumber, liveOnly);

            var results = new ResultsModel
                              {
                                  ReportTitle = new NameModel($"Board differences between board {boardCode1.ToUpper()} revision {revisionCode1.ToUpper()} and board {boardCode2.ToUpper()} revision {revisionCode2.ToUpper()}")
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

            var com1 = components1.Select(
                a => new BoardCrefReportModel
                         {
                             CRef = a.CRef,
                             PartNumber = a.PartNumber,
                             BoardLine = a.BoardLine,
                             Sequence = 1,
                             AssemblyTechnology = a.AssemblyTechnology,
                             Quantity = a.Quantity
                         });
            var com2 = components2.Select(
                a => new BoardCrefReportModel
                         {
                             CRef = a.CRef,
                             PartNumber = a.PartNumber,
                             BoardLine = a.BoardLine,
                             Sequence = 2,
                             AssemblyTechnology = a.AssemblyTechnology,
                             Quantity = a.Quantity
                         });

            var combined = com1.Concat(com2);
            var crefGroups = combined.GroupBy(a => a.CRef);

            foreach (var crefGroup in crefGroups)
            {
                var crefGroupOrdered = crefGroup.OrderBy(a => a.Sequence).ThenBy(b => b.BoardLine).ToList();
                if (crefGroupOrdered.Count >= 2 && crefGroupOrdered.First().PartNumber != crefGroupOrdered.Last().PartNumber)
                {
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "partNumber1",
                                       TextDisplay = crefGroupOrdered.First().PartNumber
                    });
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "assemblyTech1",
                                       TextDisplay = crefGroupOrdered.First().AssemblyTechnology
                                   });
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "qty1",
                                       TextDisplay = crefGroupOrdered.First().Quantity.ToString("F1")
                                   });
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "partNumber2",
                                       TextDisplay = crefGroupOrdered.Last().PartNumber
                                   });
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "assemblyTech2",
                                       TextDisplay = crefGroupOrdered.Last().AssemblyTechnology
                                   });
                    valueModels.Add(new CalculationValueModel
                                   {
                                       RowId = crefGroup.Key,
                                       ColumnId = "qty2",
                                       TextDisplay = crefGroupOrdered.Last().Quantity.ToString("F1")
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

        public ResultsModel GetBomDifferencesReport(
            string bom1, 
            string bom2, 
            bool singleLevel = true, 
            bool showDescriptions = false)
        {

            IEnumerable<BomDetailViewEntry> first;

            IEnumerable<BomDetailViewEntry> second;

            if (singleLevel)
            {
                first = this.bomDetailViewRepository
                    .FilterBy(x => x.ChangeState == "LIVE" && x.BomPartNumber == bom1).ToList();
                second = this.bomDetailViewRepository
                    .FilterBy(x => x.ChangeState == "LIVE" && x.BomPartNumber == bom2).ToList();
            }
            else
            {
                first = this.bomTreeService.FlattenBomTree(bom1, 0, false, false).Select(
                    x => new BomDetailViewEntry
                             {
                                 PartNumber = x.Name,
                                 Qty = x.Qty,
                                 Part = new Part
                                            {
                                                ExpectedUnitPrice = x.Cost,
                                                Description = x.Description
                                            }
                             }).Where(d => d.PartNumber != bom1).ToList();
                second = this.bomTreeService.FlattenBomTree(bom2, 0, false, false)
                    .Select(x => new BomDetailViewEntry
                                     {
                                         PartNumber = x.Name,
                                         Qty = x.Qty, Part = new Part
                                                                 {
                                                                     ExpectedUnitPrice = x.Cost,
                                                                     Description = x.Description
                                                                 }
                                     }).Where(d => d.PartNumber != bom2).ToList();
            }
            
            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);
            var cols = new List<AxisDetailsModel>
                           {
                               new ("PartNumber1", bom1, GridDisplayType.TextValue),
                           };

            if (showDescriptions)
            {
                cols.Add(new AxisDetailsModel("Desc1", "Desc", GridDisplayType.TextValue));
            }

            cols.Add(new AxisDetailsModel("Qty1", "Qty", GridDisplayType.TextValue));
            cols.Add(new AxisDetailsModel("Cost1", "Cost", GridDisplayType.TextValue));
            cols.Add(new AxisDetailsModel("PartNumber2", bom2, GridDisplayType.TextValue));
            
            if (showDescriptions)
            {
                cols.Add(new AxisDetailsModel("Desc2", "Desc", GridDisplayType.TextValue));
            }

            cols.Add(new AxisDetailsModel("Qty2", "Qty", GridDisplayType.TextValue));
            cols.Add(new AxisDetailsModel("Cost2", "Cost", GridDisplayType.TextValue));
            cols.Add(new AxisDetailsModel("Diff", "Diff", GridDisplayType.Value)
                         {
                             DecimalPlaces = 5
                         });
            
            reportLayout.AddColumnComponent(null, cols);

            reportLayout.ReportTitle = singleLevel ? $"Single Level BOM differences between {bom1.ToUpper()} and {bom2.ToUpper()}"
                                           : $"Flattened BOM differences between {bom1.ToUpper()} and {bom2.ToUpper()}";

            var values = new List<CalculationValueModel>();
            var diffTotal = 0m;

            var inBoth = new List<BomDetailViewEntry>();

            foreach (var detail in first.DistinctBy(z => z.PartNumber))
            {
                var inSecond = second.Where(x => x.PartNumber == detail.PartNumber).ToList();
                var inFirst = first.Where(x => x.PartNumber == detail.PartNumber).ToList();
                
                if (!inSecond.Any())
                {
                    // parts only in the first bom
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "PartNumber1",
                                TextDisplay = detail.PartNumber
                            });
                    if (showDescriptions)
                    {
                        values.Add(
                            new CalculationValueModel
                                {
                                    RowId = detail.PartNumber,
                                    ColumnId = "Desc1",
                                    TextDisplay = detail.Part.Description
                                });
                    }
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "Qty1",
                                TextDisplay = inFirst.Sum(x => x.Qty).ToString("0.#####")
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "Cost1",
                                TextDisplay 
                                    = (detail.Qty * detail.Part.ExpectedUnitPrice.GetValueOrDefault()).ToString("0.#####")
                        });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "PartNumber2"
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "Qty2"
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "Cost2"
                            });
                    var diff = 0 - detail.Qty * detail.Part.ExpectedUnitPrice.GetValueOrDefault();
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "Diff",
                                Value = diff
                            });
                    diffTotal += diff;
                }
                else if (inSecond.Sum(x => x.Qty) == inFirst.Sum(x => x.Qty))
                {
                    continue;
                }
                else
                {
                    // part is in both boms
                    inBoth.Add(detail);
                }
            }

            foreach (var detail in inBoth)
            {
                    var inSecond = second.Where(x => x.PartNumber == detail.PartNumber).ToList();
                    var inFirst = first.Where(x => x.PartNumber == detail.PartNumber).ToList();
                    var cost1 = inFirst.First().Part.ExpectedUnitPrice.GetValueOrDefault() * inFirst.Sum(x => x.Qty);
                    var cost2 = inSecond.First().Part.ExpectedUnitPrice.GetValueOrDefault() * inSecond.Sum(x => x.Qty);
                    
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "PartNumber1",
                                TextDisplay = detail.PartNumber
                            });
                    if (showDescriptions)
                    {
                        values.Add(
                            new CalculationValueModel
                                {
                                    RowId = detail.PartNumber,
                                    ColumnId = "Desc1",
                                    TextDisplay = detail.Part.Description
                                });
                    }
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "Qty1",
                                TextDisplay = inFirst.Sum(x => x.Qty).ToString("0.#####")
                        });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "Cost1",
                                TextDisplay = cost1.ToString("0.#####")
                        });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "PartNumber2",
                                TextDisplay = detail.PartNumber
                            });
                    if (showDescriptions)
                    {
                        values.Add(
                            new CalculationValueModel
                                {
                                    RowId = detail.PartNumber,
                                    ColumnId = "Desc2",
                                    TextDisplay = detail.Part.Description
                                });
                    }
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "Qty2",
                                TextDisplay = inSecond.Sum(x => x.Qty).ToString("0.#####")
                        });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "Cost2",
                                TextDisplay = cost2.ToString("0.#####")
                        });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = detail.PartNumber,
                                ColumnId = "Diff",
                                Value = cost2 - cost1
                            });
                    diffTotal += cost2 - cost1;
            }

            foreach (var detail in second.DistinctBy(x => x.PartNumber))
            {
                // parts only in the second bom
                var inFirst = first.Where(x => x.PartNumber == detail.PartNumber);
                var inSecond = second.Where(x => x.PartNumber == detail.PartNumber);
                if (!inFirst.Any())
                {
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = detail.PartNumber,
                            ColumnId = "PartNumber1"
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = detail.PartNumber,
                            ColumnId = "Qty1"
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = detail.PartNumber,
                            ColumnId = "Cost1"
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = detail.PartNumber,
                            ColumnId = "PartNumber2",
                            TextDisplay = detail.PartNumber
                        });
                    if (showDescriptions)
                    {
                        values.Add(
                            new CalculationValueModel
                                {
                                    RowId = detail.PartNumber,
                                    ColumnId = "Desc2",
                                    TextDisplay = detail.Part.Description
                                });
                    }
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = detail.PartNumber,
                            ColumnId = "Qty2",
                            TextDisplay = detail.Qty.ToString("0.#####")
                        });
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = detail.PartNumber,
                            ColumnId = "Cost2",
                            TextDisplay = (inSecond.Sum(x => x.Qty) * detail.Part.ExpectedUnitPrice.GetValueOrDefault()).ToString("0.#####")
                        });
                    var diff = 0 - detail.Qty * detail.Part.ExpectedUnitPrice.GetValueOrDefault();
                    values.Add(
                        new CalculationValueModel
                        {
                            RowId = detail.PartNumber,
                            ColumnId = "Diff",
                            Value = diff
                        });
                    diffTotal += diff;
                }
            }
            
            values.Add(
                new CalculationValueModel
                    {
                        RowId = "DiffTotal",
                        ColumnId = "Diff",
                        Value = diffTotal
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = "Totals",
                        ColumnId = "PartNumber1",
                        TextDisplay = "Mat Price"
                    });
            var part1 = this.partRepository.FindBy(p => p.PartNumber == bom1);
            var part2 = this.partRepository.FindBy(p => p.PartNumber == bom2);

            values.Add(
                new CalculationValueModel
                    {
                        RowId = "Totals",
                        ColumnId = "Cost1",
                        TextDisplay = part1.MaterialPrice.GetValueOrDefault().ToString("0.#####")
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = "Totals",
                        ColumnId = "PartNumber2",
                        TextDisplay = "Mat Price"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = "Totals",
                        ColumnId = "Cost2",
                        TextDisplay = part2.MaterialPrice.GetValueOrDefault().ToString("0.#####")
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = "Totals",
                        ColumnId = "Diff",
                        Value = part2.MaterialPrice.GetValueOrDefault() 
                                - part1.MaterialPrice.GetValueOrDefault()
                    });

            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }

        public ResultsModel GetBoardComponentSummaryReport(string boardCode, string revisionCode)
        {
            if (string.IsNullOrWhiteSpace(boardCode) || string.IsNullOrWhiteSpace(revisionCode))
            {
                throw new InvalidOptionException("Board or revision not specified");
            }

            var board = this.boardRepository.FindById(boardCode.ToUpper());

            if (board == null)
            {
                throw new ItemNotFoundException($"{boardCode.ToUpper()} could not be found");
            }

            var revision = board.Layouts.SelectMany(a => a.Revisions)
                .First(a => a.RevisionCode == revisionCode.ToUpper());
            var components = board.ComponentsOnRevision(revision.LayoutSequence, revision.VersionNumber);

            var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);

            var results = new ResultsModel
                              {
                                  ReportTitle = new NameModel(
                                      $"Board Component Summary Report - Board Code : {boardCode.ToUpper()} Revision : {revisionCode.ToUpper()}")
                              };
            results.AddSortedColumns(
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("cRef", "CRef", GridDisplayType.TextValue),
                        new AxisDetailsModel("boardDescription", "Board Description", GridDisplayType.TextValue),
                        new AxisDetailsModel("partNumber", "Part Number", GridDisplayType.TextValue),
                        new AxisDetailsModel("description", "Description", GridDisplayType.TextValue),
                        new AxisDetailsModel("qty", "Qty", GridDisplayType.TextValue),
                        new AxisDetailsModel("ourUnitOfMeasure", "Our Unit of Measure", GridDisplayType.TextValue),
                        new AxisDetailsModel("assemblyTechnology", "Assembly Technology", GridDisplayType.TextValue),
                        new AxisDetailsModel("bomPartNumber", "Bom Part Number", GridDisplayType.TextValue),
                        new AxisDetailsModel("pcasPartNumber", "PCAS Part Number", GridDisplayType.TextValue),
                        new AxisDetailsModel("pcsmPartNumber", "PCSM Part Number", GridDisplayType.TextValue),
                        new AxisDetailsModel("pcbPartNumber", "PCB Part Number", GridDisplayType.TextValue),
                        new AxisDetailsModel("versionNumber", "Version Number", GridDisplayType.TextValue),
                        new AxisDetailsModel("splitBom", "Split Bom", GridDisplayType.TextValue)
                    });

            var values = new List<CalculationValueModel>();

            foreach (var boardComponent in components)
            {
                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "cRef",
                            TextDisplay = boardComponent.CRef
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "boardDescription",
                            TextDisplay = board.Description
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "partNumber",
                            TextDisplay = boardComponent.PartNumber
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "description",
                            TextDisplay = boardComponent.Part.Description
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "ourUnitOfMeasure",
                            TextDisplay = boardComponent.Part.OurUnitOfMeasure
                    });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "assemblyTechnology",
                            TextDisplay = boardComponent.AssemblyTechnology
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "pcasPartNumber",
                            TextDisplay = revision.PcasPartNumber
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "pcbPartNumber",
                            TextDisplay = revision.PcbPartNumber
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "pcsmPartNumber",
                            TextDisplay = revision.PcsmPartNumber
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "pcsmPartNumber",
                            TextDisplay = revision.VersionNumber.ToString()
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = boardComponent.CRef,
                            ColumnId = "splitBom",
                            TextDisplay = revision.SplitBom
                        });
            }

            reportLayout.SetGridData(values);
            return reportLayout.GetResultsModel();
        }

        public IEnumerable<ResultsModel> GetBomPrintReport(string bomName)
        {
            var results = new List<ResultsModel>();
            var allDetailsOnBom = this.bomTreeService.FlattenBomTree(bomName, null, false).ToList();
            var componentIds = allDetailsOnBom.Where(d => d.Type == "C").Select(c => int.Parse(c.Id)).ToList();
            var components = this.bomDetailComponents.FilterBy(
                x => componentIds
                    .Contains(x.DetailId)).ToList();
            var assemblies = allDetailsOnBom.Where(x => x.Type != "C");

            foreach (var assembly in assemblies)
            {
                var reportLayout = new SimpleGridLayout(this.reportingHelper, CalculationValueModelType.Value, null, null);
                reportLayout.ReportTitle = assembly.Name;
                reportLayout.AddColumnComponent(null, new List<AxisDetailsModel>
                                                          {
                                                              new AxisDetailsModel("Cref", "Cref", GridDisplayType.TextValue) { SortOrder = 1 },
                                                              new AxisDetailsModel("Part", "Part", GridDisplayType.TextValue),
                                                              new AxisDetailsModel("Description", "Description", GridDisplayType.TextValue),
                                                          });
                
                var values = new List<CalculationValueModel>();

                var thisDetailIds = assembly.Children.Where(x => x.Type == "C").ToList().Select(d => int.Parse(d.Id));
                var thisComponents = components.Where(x => thisDetailIds.Contains(x.DetailId)).OrderBy(c => c.CircuitRef);
                var nonComponents = assembly.Children.ToList().Where(
                    x => x.Type == "C" && !thisComponents.Select(c => c.DetailId)
                             .ToList().Contains(int.Parse(x.Id))).OrderBy(n => n.Name);

                foreach (var n in nonComponents)
                {
                    var rowId = n.Id;
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "Cref",
                                TextDisplay = string.Empty
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "Part",
                                TextDisplay = n.Name
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "Description",
                                TextDisplay = n.Description
                            });
                }
                foreach (var c in thisComponents)
                {
                    var rowId = c.DetailId.ToString();
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "Cref",
                                TextDisplay = c.CircuitRef
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "Part",
                                TextDisplay = c.Component
                            });
                    values.Add(
                        new CalculationValueModel
                            {
                                RowId = rowId,
                                ColumnId = "Description",
                                TextDisplay = c.DetailViewEntry?.Part?.Description
                            });
                }

                reportLayout.SetGridData(values);
                results.Add(reportLayout.GetResultsModel());
            }
 
            return results;
        }
    }
}
