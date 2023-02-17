namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class BomHistoryReportService : IBomHistoryReportService
    {
        private readonly IBomTreeService treeService;

        private readonly IQueryRepository<BomHistoryViewEntry> bomHistoryRepository;

        public BomHistoryReportService(
            IBomTreeService treeService,
            IQueryRepository<BomHistoryViewEntry> bomHistoryRepository)
        {
            this.treeService = treeService;
            this.bomHistoryRepository = bomHistoryRepository;
        }

        public IEnumerable<BomHistoryReportLine> GetReport(string bomName, DateTime from, DateTime to)
        {
            return this.bomHistoryRepository
                .FilterBy(x => x.DateApplied >= from && x.DateApplied <= to && x.BomName == bomName)
                .ToList()
                .OrderBy(x => x.ChangeId)
                .ThenBy(x => x.DetailId)
                .ThenByDescending(x => x.Operation)
                .GroupBy(x => x.ChangeId)
                .Select(
                    g => new BomHistoryReportLine
                             {
                        ChangeId = g.Key,
                        DetailId =
                            g.First().DetailId
                            + (g.Count() > 1 ? Environment.NewLine + g.ElementAt(1).DetailId : string.Empty),
                        DocumentNumber = g.First().DocumentNumber,
                        BomName = g.First().BomName,
                        DateApplied = g.First().DateApplied,
                        AppliedBy = g.First().AppliedBy,
                        DocumentType = g.First().DocumentType,
                        Operation =
                            g.First().Operation
                            + (g.Count() > 1 ? Environment.NewLine + g.ElementAt(1).Operation : string.Empty),
                        PartNumber =
                            g.First().PartNumber
                            + (g.Count() > 1 ? Environment.NewLine + g.ElementAt(1).PartNumber : string.Empty),
                        Qty = g.First().Qty + (g.Count() > 1
                                                   ? Environment.NewLine + g.ElementAt(1).Qty
                                                   : string.Empty),
                        GenerateRequirement = g.First().GenerateRequirement
                                              + (g.Count() > 1
                                                     ? Environment.NewLine + g.ElementAt(1).GenerateRequirement
                                                     : string.Empty)
                    });
        }

        public IEnumerable<BomHistoryReportLine> GetReportWithSubAssemblies(string bomName, DateTime from, DateTime to)
        {
            var subAssemblies = this.treeService.FlattenBomTree(bomName, null, false)
                .Where(x => x.Type != "C")
                .Select(x => x.Name);

            var changeGroups = this.bomHistoryRepository
                .FilterBy(x => x.DateApplied >= from && x.DateApplied <= to && subAssemblies.Contains(x.BomName))
                .ToList()
                .OrderBy(x => x.ChangeId)
                .ThenBy(x => x.DetailId)
                .ThenByDescending(x => x.Operation)
                .GroupBy(x => x.ChangeId);

            return changeGroups.Select(
                g => new BomHistoryReportLine
                         {
                             ChangeId = g.Key,
                             DetailId =
                                 g.First().DetailId
                                 + (g.Count() > 1 ? Environment.NewLine + g.ElementAt(1).DetailId : string.Empty),
                             DocumentNumber = g.First().DocumentNumber,
                             BomName = g.First().BomName,
                             DateApplied = g.First().DateApplied,
                             AppliedBy = g.First().AppliedBy,
                             DocumentType = g.First().DocumentType,
                             Operation =
                                 g.First().Operation
                                 + (g.Count() > 1 ? Environment.NewLine + g.ElementAt(1).Operation : string.Empty),
                             PartNumber =
                                 g.First().PartNumber
                                 + (g.Count() > 1 ? Environment.NewLine + g.ElementAt(1).PartNumber : string.Empty),
                             Qty = g.First().Qty + (g.Count() > 1
                                                        ? Environment.NewLine + g.ElementAt(1).Qty
                                                        : string.Empty),
                             GenerateRequirement = g.First().GenerateRequirement
                                                   + (g.Count() > 1
                                                          ? Environment.NewLine + g.ElementAt(1).GenerateRequirement
                                                          : string.Empty)
                         });
        }
    }
}