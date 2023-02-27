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
            var endOfToDate = to.Date.AddDays(1).AddTicks(-1);
            var assemblies = this.GetEveryAssemblyThatWasEverOnBom(bomId).ToList();
            var names = assemblies.Select(a => a.Name).ToList();
            
            var changes = this.changeRepository.FilterBy(
                x => (x.BomName == bomName || names.Contains(x.BomName)) 
                     && x.DateApplied >= from && x.DateApplied <= endOfToDate).ToList();

            var changesToIgnore = new List<int>();

            foreach (var bomChange in changes)
            {
                var assembly = assemblies.Where(a => bomChange.BomName == a.Name);
                var curr = assembly.FirstOrDefault();
                while (curr?.ParentId != null)
                {
                    if (curr.AddedOn < curr.Parent.AddedOn)
                    {
                        changesToIgnore.Add(bomChange.ChangeId);
                        break;
                    }

                    curr = curr.Parent;
                }
            }

            changes = changes.Where(x => !changesToIgnore.Contains(x.ChangeId))
                .ToList();

            var changeGroups = this.bomHistoryRepository
                .FilterBy(x => x.DateApplied >= from && x.DateApplied <= endOfToDate).ToList().Join(
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

        private IEnumerable<BomTreeNode> GetEveryAssemblyThatWasEverOnBom(int bomId)
        {
            var result = new List<BomTreeNode>();
            var firstLevel = this.detailRepository.FilterBy(x => x.BomId == bomId && x.Part.BomType != "C" && x.ChangeState != "CANCEL");

            var stack = new Stack<BomTreeNode>();

            stack.Push(
                new BomTreeNode
                    {
                        PartBomId = bomId,
                        Children = firstLevel.Select(
                            d => new BomTreeNode
                                     {
                                         Name = d.PartNumber,
                                         AddChangeId = d.AddChangeId,
                                         ChangeState = d.ChangeState,
                                         AddedOn = d.AddChange.DateApplied,
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
                    
                    var children = this.detailRepository.FilterBy(
                        d => d.BomId == current.PartBomId && d.ChangeState != "CANCEL" && d.Part.BomType != "C");

                    current.Children = children.Select(
                        detail => new BomTreeNode
                                      {
                                          Name = detail.PartNumber,
                                          BomId = detail.BomId,
                                          AddChangeId = detail.AddChangeId,
                                          AddedOn = detail.AddChange.DateApplied,
                                          ChangeState = detail.ChangeState,
                                          Type = detail.Part.BomType,
                                          PartBomId = detail.Part.BomId,
                                          ParentId = current.Id,
                                          ParentName = current.Name,
                                          Parent = current,
                                          Id = detail.DetailId.ToString()
                                      }).ToList();

                    if (current.Children != null)
                    {
                        foreach (var child in current.Children)
                        {
                            if (!result.Any(x => x.Name == child.Name && x.ParentName == child.ParentName))
                            {
                                stack.Push(child);
                            }

                            result.Add(child);
                        }
                    }

                    numChildren--;
                }
            }

            return result;
        }

        // traverse the tree of any assemblies that were on this bom during the specified date range
        private IEnumerable<BomChange> GetChangesRelevantToHistory(int bomId, DateTime from, DateTime to)
        {
            var result = new List<BomChange>();
            var firstLevel = this.detailRepository.FilterBy(
                x => x.BomId == bomId && x.Part.BomType != "C");

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
                    var children = this.detailRepository.FilterBy(
                        d => d.BomId == current.PartBomId && d.Part.BomType != "C" && d.ChangeState != "CANCEL");

                    current.Children = children
                        .Select(
                            detail => new BomTreeNode
                                          {
                                              Name = detail.PartNumber,
                                              BomId = detail.BomId,
                                              AddChangeId = detail.AddChangeId,
                                              AddedOn = detail.AddChange.DateApplied,
                                              //DeleteOn = detail.DeleteChange.DateApplied,
                                              DeleteChangeId = detail.DeleteChangeId,
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

                            // include only changes after assembly was added to the bom?
                            var changes = this.changeRepository.FilterBy(
                                x => x.ChangeState != "CANCEL" && x.BomName == child.Name && x.DateApplied >= from
                                                             && x.DateApplied <= to.AddDays(1).AddTicks(-1)
                                                             && x.DateApplied >= detail.AddChange.DateApplied);

                            // and before it was deleted from the bom (if it has in fact been deleted from the bom)
                            changes = changes.Where(
                                x => detail.DeleteChange == null || x.DateApplied < detail.DeleteChange.DateApplied);
                            
                            if (child.ParentId != null)
                            {
                                // and after its parent was added to the bom
                                var parent = this.detailRepository.FindById(int.Parse(child.ParentId));
                                changes = changes.Where(c => c.DateApplied >= parent.AddChange.DateApplied);
                            
                                // and before its parent was deleted from the bom (if its parent has in fact been delete from the bom)
                                changes = changes.Where(
                                    x => parent.DeleteChange == null || x.DateApplied < parent.DeleteChange.DateApplied);
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
