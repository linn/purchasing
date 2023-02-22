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

        private readonly IRepository<BomChange, int> changeRepository;

        private readonly IQueryRepository<BomHistoryViewEntry> bomHistoryRepository;

        public BomHistoryReportService(
            IQueryRepository<BomHistoryViewEntry> bomHistoryRepository,
            IRepository<BomDetail, int> detailRepository,
            IRepository<BomChange, int> changeRepository,
            IRepository<Bom, int> bomRepository)
        {
            this.bomRepository = bomRepository;
            this.changeRepository = changeRepository;
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
            var changes = this.GetChangesRelevantToHistory(bomId, from, to).ToList();

            changes.AddRange(this.changeRepository
                .FilterBy(c => c.BomName == bomName && c.DateApplied > from && c.DateApplied < to.AddDays(1)));

            var changeGroups = this.bomHistoryRepository
                .FilterBy(x => x.DateApplied >= from && x.DateApplied <= to).ToList().Join(
                    changes,
                    bomHistoryViewEntry => bomHistoryViewEntry.ChangeId,
                    c => c.ChangeId,
                    (bomHistoryViewEntry, _) => bomHistoryViewEntry)
                .ToList().OrderBy(x => x.ChangeId).ThenBy(x => x.DetailId).ThenByDescending(x => x.Operation)
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
                             Qty = string.Join(Environment.NewLine, g.Select(d => d.Qty.ToString())),
                             GenerateRequirement = string.Join(
                                 Environment.NewLine,
                                 g.Select(d => d.GenerateRequirement.ToString()))
                         });
        }

        private IEnumerable<BomChange> GetChangesRelevantToHistory(int bomId, DateTime from, DateTime to)
        {
            var result = new List<BomChange>();
            var firstLevel = this.detailRepository.FilterBy(
                x => x.BomId == bomId && x.ChangeState == "LIVE" && x.Part.BomType != "C");

            var stack = new Stack<BomTreeNode>();

            stack.Push(
                new BomTreeNode
                    {
                        PartBomId = bomId,
                        Children = firstLevel.Select(
                            d => new BomTreeNode
                                     {
                                         Name = d.PartNumber,
                                         ChangeState = d.ChangeState,
                                         Type = d.Part.BomType,
                                         PartBomId = d.BomId,
                                         Id = d.DetailId.ToString()
                                     })
                    });

            while (stack.Count != 0)
            {
                var numChildren = stack.Count;
                while (numChildren > 0)
                {
                    var current = stack.Pop();
                    current.Children = this.detailRepository
                        .FilterBy(d => d.BomId == current.PartBomId && d.ChangeState == "LIVE" && d.Part.BomType != "C")
                        .Select(
                            detail => new BomTreeNode
                                          {
                                              Name = detail.PartNumber,
                                              BomId = detail.BomId,
                                              ChangeState = detail.ChangeState,
                                              Type = detail.Part.BomType,
                                              PartBomId = detail.Part.BomId,
                                              ParentId = current.Id,
                                              Id = detail.DetailId.ToString()
                                          }).ToList();

                    if (current.Children != null)
                    {
                        foreach (var child in current.Children)
                        {
                            var detail = this.detailRepository.FindById(int.Parse(child.Id));

                            // does this sub assembly have any changes after it was added to the bom?
                            var changes = this.changeRepository.FilterBy(
                                x => x.BomName == child.Name && x.DateApplied >= from
                                                             && x.DateApplied <= to.AddDays(1).AddTicks(-1)
                                                             && x.DateApplied >= detail.AddChange.DateApplied);

                            // did its parent get added to the bom after those changes were made?
                            if (child.ParentId != null)
                            {
                                var parent = this.detailRepository.FindById(int.Parse(child.ParentId));
                                changes = changes.Where(c => c.DateApplied >= parent.AddChange.DateApplied);
                            }
                            
                            if (changes.Any())
                            {
                                result.AddRange(changes);
                            }

                            stack.Push(child);
                        }
                    }

                    numChildren--;
                }
            }

            return result;
        }
    }
}
