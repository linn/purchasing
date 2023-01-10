namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class BomChangeService : IBomChangeService
    {
        private readonly IDatabaseService databaseService;

        private readonly IRepository<BomChange, int> bomChangeRepository;

        private readonly IRepository<BomDetail, int> bomDetailRepository;

        private readonly IRepository<Bom, int> bomRepository;

        private readonly IQueryRepository<Part> partRepository;

        private readonly IBomPack bomPack;

        public BomChangeService(
            IDatabaseService databaseService, 
            IRepository<BomChange, int> bomChangeRepository,
            IRepository<BomDetail, int> bomDetailRepository,
            IRepository<Bom, int> bomRepository,
            IQueryRepository<Part> partRepository,
            IBomPack bomPack)
        {
            this.databaseService = databaseService;
            this.bomChangeRepository = bomChangeRepository;
            this.bomDetailRepository = bomDetailRepository;
            this.bomRepository = bomRepository;
            this.partRepository = partRepository;
            this.bomPack = bomPack;
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

                    // add a new bom_change for any bom that has changed
                    if (current.HasChanged.GetValueOrDefault() && current.Children != null)
                    {
                        // create a bom if required
                        var bom = this.bomRepository.FindBy(x => x.BomName == current.Name) ?? new Bom
                                      {
                                          BomId = this.databaseService.GetIdSequence("BOM_SEQ"),
                                          BomName = tree.Name,
                                          Part = this.partRepository.FindBy(x => x.PartNumber == tree.Name),
                                          Depth = 1,
                                          CommonBom = "N"
                                      };

                        this.bomRepository.Add(bom);
                        bom.Part.BomId = bom.BomId;
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
                            var part = this.partRepository.FindBy(x => x.PartNumber == child.Name);

                            if (part == null)
                            {
                                throw new ItemNotFoundException($"Invalid Part Number: {child.Name} on Assembly: {current.Name}");
                            }

                            // case: adding a new part that is not on this bom
                            // add a detail for any new part on the bom
                            if (bom.Details == null || bom.Details.Count == 0 || bom.Details.All(d => d.PartNumber != child.Name))
                            {
                                if (part.DatePurchPhasedOut
                                    .HasValue)
                                {
                                    throw new InvalidBomChangeException(
                                        $"Can't add {child.Name} to {child.ParentName} - part has been phased out by purchasing");
                                }

                                if (string.IsNullOrEmpty(part.DecrementRule))
                                {
                                    throw new InvalidBomChangeException(
                                        $"Can't add {child.Name} to {child.ParentName} - part has no decrement rule!");
                                }

                                if (string.IsNullOrEmpty(part.BomType))
                                {
                                    throw new InvalidBomChangeException(
                                        $"Can't add {child.Name} to {child.ParentName} - part has no BOM Type!");
                                }

                                child.ChangeState = "PROPOS";

                                // stop bom loops
                                // todo: this check will only stop us adding a part to its own bom in its first level of children
                                // but we can still add a part to its own bom one level deeper, for example, and still create a loop
                                // do we need to think of a way to stop this happening? Does the oracle form do anything more?
                                if (child.Name == current.Name)
                                {
                                    throw new InvalidBomChangeException($"Can't add {child.Name} to it's own BOM!");
                                }

                                this.bomDetailRepository.Add(new BomDetail 
                                                                 {
                                                                     DetailId = this.databaseService.GetIdSequence("BOMDET_SEQ"),
                                                                     BomId = bom.BomId,
                                                                     PartNumber = child.Name,
                                                                     Qty = child.Qty,
                                                                     GenerateRequirement = "Y", // todo  child.Requirement,
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
                                var replacement = current.Children.FirstOrDefault(c => c.ReplacementFor == child.Name);
                                var replacementPart = this.partRepository.FindBy(x => x.PartNumber == replacement.Name);

                                if (replacementPart.DatePurchPhasedOut.HasValue)
                                {
                                    throw new InvalidBomChangeException(
                                        $"Can't add {replacement.Name} to {child.ParentName} - part has been phased out by purchasing");
                                }

                                if (string.IsNullOrEmpty(replacementPart.DecrementRule))
                                {
                                    throw new InvalidBomChangeException(
                                        $"Can't add {replacement.Name} to {child.ParentName} - part has no decrement rule!");
                                }

                                if (string.IsNullOrEmpty(replacementPart.BomType))
                                {
                                    throw new InvalidBomChangeException(
                                        $"Can't add {replacement.Name} to {child.ParentName} - part has no BOM Type!");
                                }

                                if (replacement == null)
                                {
                                    throw new InvalidBomChangeException(
                                        $"{child.Name} is marked for replacement but no replacement part is specified");
                                }

                                if (child.AddChangeDocumentNumber == changeRequestNumber)
                                {
                                    throw new InvalidBomChangeException(
                                        $"{child.Name} was added by the current change request - no need to replace it - just edit it directly.");
                                }

                                // stop bom loops
                                // todo: same consideration as above in the adding case
                                if (replacement.Name == current.Name)
                                {
                                    throw new InvalidBomChangeException($"Can't add {replacement.Name} to it's own BOM!");
                                }

                                var replacedDetail = this.bomDetailRepository.FindById(int.Parse(child.Id));

                                if (replacedDetail.PcasLine == "Y")
                                {
                                    throw new InvalidBomChangeException(
                                        $"{child.Name} is a PCAS line - cannot replace here.");
                                }

                                replacement.AddReplaceSeq = replacementSeq;
                                child.DeleteReplaceSeq = replacementSeq;

                                replacedDetail.DeleteChangeId = id;
                                replacedDetail.DeleteReplaceSeq = replacementSeq;
                                replacedDetail.ChangeState = "PROPOS";
                            }

                            // case: deleting a part from the bom
                            if (child.ToDelete.GetValueOrDefault())
                            {
                                var toDelete = this.bomDetailRepository.FindById(int.Parse(child.Id));

                                if (toDelete.DeleteChangeId.HasValue 
                                    && toDelete.DeleteChange?.DocumentNumber != changeRequestNumber)
                                {
                                    throw new InvalidBomChangeException(
                                        $"{child.Name} is already marked for deletion by another change request" 
                                        + $" ({toDelete.DeleteChange?.DocumentNumber})");
                                }

                                if (toDelete.PcasLine == "Y")
                                {
                                    throw new InvalidBomChangeException($"{child.Name} is a PCAS line - cannot delete here.");
                                }

                                toDelete.DeleteChangeId = id;
                            }
                        }

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

        public void CopyBom(string srcPartNumber, int destBomId, int changedBy, int crfNumber)
        {
            var changeId = this.databaseService.GetIdSequence("CHG_SEQ"); // not always right
            // we need to look up the change if one exists for this bom
            // or create a new one?
            this.bomPack.CopyBom(srcPartNumber, destBomId, changeId, "PROPOS", "O");
        }
    }
}
