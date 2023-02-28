namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class BomHistoryReportService : IBomHistoryReportService
    {
        private readonly IRepository<Bom, int> bomRepository;

        private readonly IRepository<BomDetail, int> detailRepository;

        private readonly IQueryRepository<BomHistoryViewEntry> bomHistoryRepository;

        public BomHistoryReportService(
            IQueryRepository<BomHistoryViewEntry> bomHistoryRepository,
            IRepository<BomDetail, int> detailRepository,
            IRepository<Bom, int> bomRepository)
        {
            this.bomRepository = bomRepository;
            this.bomHistoryRepository = bomHistoryRepository;
            this.detailRepository = detailRepository;
        }

        public IEnumerable<BomHistoryReportLine> GetReport(string bomName, DateTime from, DateTime to)
        {
            return this.bomHistoryRepository
                .FilterBy(x => x.DateApplied >= from && x.DateApplied <= to && x.BomName == bomName).ToList()
                .OrderBy(x => x.ChangeId).ThenBy(x => x.DetailId).ThenByDescending(x => x.Operation)
                .GroupBy(x => x.ChangeId).Select(
                    g => new BomHistoryReportLine
                             {
                                 ChangeId = g.Key,
                                 DetailId = string.Join(Environment.NewLine, g.Select(d => d.DetailId.ToString())),
                                 DocumentNumber = g.First().DocumentNumber,
                                 BomName = g.First().BomName,
                                 DateApplied = g.First().DateApplied,
                                 AppliedBy = g.First().AppliedBy,
                                 DocumentType = g.First().DocumentType,
                                 Operation = string.Join(Environment.NewLine, g.Select(d => d.Operation.ToString())),
                                 PartNumber = string.Join(Environment.NewLine, g.Select(d => d.PartNumber.ToString())),
                                 Qty = string.Join(Environment.NewLine, g.Select(d => d.Qty.ToString())),
                                 GenerateRequirement = string.Join(
                                     Environment.NewLine,
                                     g.Select(d => d.GenerateRequirement.ToString()))
                             });
        }

        public IEnumerable<BomHistoryReportLine> GetReportWithSubAssemblies(string bomName, DateTime from, DateTime to)
        {
            var bomId = this.bomRepository.FindBy(x => x.BomName == bomName).BomId;
            var endOfToDate = to.Date.AddDays(1).AddTicks(-1);
            var subAssemblies = new List<BomTreeNode>();

            this.IncludeSubAssemblies(bomId, from, endOfToDate, subAssemblies);

            subAssemblies.Add(new BomTreeNode { Name = bomName, AddedOn = from, DeletedOn = endOfToDate });

            var changeGroups = this.bomHistoryRepository.FindAll()
                .Where(x => x.DateApplied >= from && x.DateApplied <= endOfToDate)
                .ToList()
                .Join(
                    subAssemblies,
                    bomHistoryViewEntry => bomHistoryViewEntry.BomName,
                    subAssembly => subAssembly.Name,
                    (hist, sub) => new { t1 = hist, t2 = sub })
                .Where(o => o.t1.BomName == o.t2.Name && o.t1.DateApplied > o.t2.AddedOn && o.t1.DateApplied < o.t2.DeletedOn)
                .Select(g => g.t1)
                .OrderBy(x => x.ChangeId).ThenBy(x => x.DetailId).ThenByDescending(x => x.Operation)
                .GroupBy(x => x.ChangeId).ToList();
            
            return changeGroups.Select(
                g => new BomHistoryReportLine 
                         {
                             ChangeId = g.Key,
                             DetailId = string.Join(Environment.NewLine, g.Select(d => d.DetailId.ToString())),
                             DocumentNumber = g.First().DocumentNumber,
                             BomName = g.First().BomName,
                             DateApplied = g.First().DateApplied,
                             AppliedBy = g.First().AppliedBy,
                             DocumentType = g.First().DocumentType,
                             Operation = string.Join(Environment.NewLine, g.Select(d => d.Operation.ToString())),
                             PartNumber = string.Join(Environment.NewLine, g.Select(d => d.PartNumber.ToString())),
                             Qty = string.Join(Environment.NewLine, g.Select(d => d.Qty?.ToString())),
                             GenerateRequirement = string.Join(
                                 Environment.NewLine,
                                 g.Select(d => d.GenerateRequirement?.ToString()))
                         });
        }

        private void IncludeSubAssemblies(int bomId, DateTime from, DateTime? to, ICollection<BomTreeNode> result)
        {
            var details1 = this.detailRepository.FindAll();
                
            var details = details1.Where(
                x => x.BomId == bomId
                     && (x.ChangeState == "LIVE" || x.ChangeState == "HIST")
                     && x.AddChange.DateApplied <= to
                     && (x.DeleteChange == null || x.DeleteChange.DateApplied >= from)
                     && (x.DeleteChange != null || to >= from));

            foreach (var detail in details)
            {
                if (detail.Part.BomId.HasValue)
                {
                    var removed = detail.DeleteChange?.DateApplied ?? to;

                    var added = detail.AddChange.DateApplied.GetValueOrDefault() 
                                < from ? from : detail.AddChange.DateApplied.GetValueOrDefault();
                    
                    if (removed > to)
                    {
                        removed = to;
                    }


                    if (detail.AddChange.DateApplied < from)
                    {
                        added = from;
                    }

                    result.Add(new BomTreeNode
                                   {
                                       Name = detail.PartNumber, 
                                       AddedOn = added, 
                                       DeletedOn = removed, 
                                       AddChangeId = detail.AddChangeId, 
                                       DeleteChangeId = detail.DeleteChangeId
                                   });

                    this.IncludeSubAssemblies((int)detail.Part.BomId, added, removed, result);
                }
            }
        }
    }
}
