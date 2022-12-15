namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class BomChangeService : IBomChangeService
    {
        private readonly IDatabaseService databaseService;

        private readonly IRepository<BomChange, int> bomChangeRepository;

        private readonly IRepository<BomDetail, int> bomDetailRepository;

        private readonly IRepository<Bom, int> bomRepository;

        public BomChangeService(
            IDatabaseService databaseService, 
            IRepository<BomChange, int> bomChangeRepository,
            IRepository<BomDetail, int> bomDetailRepository,
            IRepository<Bom, int> bomRepository)
        {
            this.databaseService = databaseService;
            this.bomChangeRepository = bomChangeRepository;
            this.bomDetailRepository = bomDetailRepository;
            this.bomRepository = bomRepository;
        }

        public BomTreeNode CreateBomChanges(BomTreeNode tree, int changeRequestNumber, int enteredBy)
        {
            // traversing the updated tree...
            var q = new Queue<BomTreeNode>();
            q.Enqueue(tree);
            while (q.Count != 0)
            {
                var n = q.Count;

                while (n > 0)
                {
                    var current = q.Dequeue();

                    // add a new bom_change for any bom that has changed - db triggers will create the bom if required
                    if (current.HasChanged.GetValueOrDefault())
                    {
                        var bom = this.bomRepository.FindBy(x => x.BomName == current.Name);

                        var id = this.databaseService.GetIdSequence("CHG_SEQ");
                        var change = new BomChange
                                         {
                                             BomId = bom.BomId,
                                             ChangeId = id,
                                             BomName = current.Name,
                                             DocumentType = "CRF", // for now
                                             DocumentNumber = changeRequestNumber,
                                             PartNumber = current.Name,
                                             DateEntered = DateTime.Today,
                                             EnteredById = enteredBy,
                                             ChangeState = "PROPOS",
                                             Comments = "BOM_UT",
                                             PcasChange = "N"
                                         };
                        this.bomChangeRepository.Add(change);

                        var replacementSeq = 1;

                        foreach (var child in current.Children)
                        {
                            // case: adding a new part that is not on this bom
                            // add a detail for any new part on the bom
                            if (bom.Details.All(d => d.PartNumber != child.Name && d.ChangeState == "LIVE"))
                            {
                                child.ChangeState = "PROPOS";
                                this.bomDetailRepository.Add(new BomDetail
                                                                  {
                                                                      DetailId = this.databaseService.GetIdSequence("BOMDET_SEQ"),
                                                                      BomId = bom.BomId,
                                                                      PartNumber = child.Name,
                                                                      Qty = child.Qty,
                                                                      GenerateRequirement = "Y", // todo
                                                                      ChangeState = "PROPOS",
                                                                      AddChangeId = id,
                                                                      AddReplaceSeq = string.IsNullOrEmpty(child.ReplacementFor) 
                                                                          ? null : replacementSeq++,
                                                                      DeleteChangeId = null,
                                                                      DeleteReplaceSeq = null,
                                                                      PcasLine = "N"
                                                                  });
                            }

                            // case: replacing a part on this bom with another part
                            if (!string.IsNullOrEmpty(child.ReplacedBy))
                            {
                                var replacement =
                                    current.Children.FirstOrDefault(c => c.ReplacementFor == child.Name);

                                if (replacement == null)
                                {
                                    throw new InvalidReplacementException(
                                        $"{child.Name} is marked for replacement but no replacement part is specified");
                                }

                                replacement.AddReplaceSeq = replacementSeq;
                                child.DeleteReplaceSeq = replacementSeq;

                                var replacedDetail = this.bomDetailRepository.FindById(int.Parse(child.Id));
                                replacedDetail.DeleteChangeId = id;
                                replacedDetail.DeleteReplaceSeq = replacementSeq;
                                replacedDetail.ChangeState = "PROPOS";
                            }
                        }
                    }

                    if (current.Children != null)
                    {
                        for (var i = 0; i < current.Children.Count(); i++)
                        {
                            if (current.Children?.Count() > 0)
                            {
                                q.Enqueue(current.Children.ElementAt(i));
                            }
                        }
                    }

                    n--;
                }
            }

            return tree;
        }
    }
}
