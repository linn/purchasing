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

                    if (current.HasChanged.GetValueOrDefault() && current.Children != null)
                    {
                        var bomLookup = this.bomRepository.FindBy(x => x.BomName == current.Name);

                        // create a bom if required
                        var bom = bomLookup ?? new Bom
                                      {
                                          BomId = this.databaseService.GetIdSequence("BOM_SEQ"),
                                          BomName = tree.Name,
                                          Part = this.partRepository.FindBy(x => x.PartNumber == tree.Name),
                                          Depth = 1,
                                          CommonBom = "N"
                                      };

                        if (bomLookup == null)
                        {
                            this.bomRepository.Add(bom);
                            bom.Part.BomId = bom.BomId;
                        }

                        // obtain a bom_change to make incoming changes against
                        // check if there's an open one for this bom
                        var change = this.bomChangeRepository.FindBy(
                            x => x.DocumentNumber == changeRequestNumber 
                                 && new[] { "ACCEPT", "PROPOS" }.Contains(x.ChangeState) // todo - can we amend an ACCEPTed change?
                                 && x.BomName == current.Name);

                        // create a new bom change if not
                        if (change == null)
                        {
                            var id = this.databaseService.GetIdSequence("CHG_SEQ");
                            change = new BomChange
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
                        }

                        var detailsOnChange = this.bomDetailRepository
                            .FilterBy(x => x.BomId == bom.BomId && x.AddChangeId == change.ChangeId);
                        
                        var replacementSeq = !detailsOnChange.Any() ? 0 
                                                 : detailsOnChange.Max(d => d.AddReplaceSeq.GetValueOrDefault());

                        foreach (var child in current.Children)
                        {
                            var part = this.partRepository.FindBy(x => x.PartNumber == child.Name);

                            if (part == null)
                            {
                                throw new ItemNotFoundException($"Invalid Part Number: {child.Name} on Assembly: {current.Name}");
                            }

                            if (child.ToDelete.GetValueOrDefault())
                            {
                                // case: deleting a part from the bom
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

                                toDelete.DeleteChangeId = change.ChangeId;

                                child.DeleteChangeDocumentNumber = change.DocumentNumber;
                            }
                            else
                            {
                                if (bom.Details != null 
                                    && string.IsNullOrEmpty(child.ReplacedBy)
                                    && bom.Details.Select(x => x.DetailId.ToString()).Contains(child.Id))
                                {
                                    // case: updating fields of existing detail on this bom
                                    // can only do this if part was added by current crf
                                    var toUpdate = this.bomDetailRepository.FindById(int.Parse(child.Id));

                                    if (toUpdate.Qty != child.Qty || toUpdate.GenerateRequirement != child.Requirement)
                                    {
                                        if (toUpdate.AddChangeId != change.ChangeId)
                                        {
                                            throw new InvalidBomChangeException(
                                                "Can't directly update details added by a different CRF - Replace them instead!");
                                        }

                                        toUpdate.Qty = child.Qty;
                                        toUpdate.GenerateRequirement = child.Requirement;
                                    }
                                }

                                if (bom.Details == null || bom.Details.Count == 0 || bom.Details.All(d => d.DetailId.ToString() != child.Id))
                                {
                                    // case: adding a new detail
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
                                        GenerateRequirement = child.Requirement,
                                        ChangeState = "PROPOS",
                                        AddChangeId = change.ChangeId,
                                        AddReplaceSeq = string.IsNullOrEmpty(child.ReplacementFor)
                                                                             ? null : replacementSeq + 1,
                                        DeleteChangeId = null,
                                        DeleteReplaceSeq = null,
                                        PcasLine = "N"
                                    });
                                    child.AddChangeDocumentNumber = change.DocumentNumber;
                                }

                                if (!string.IsNullOrEmpty(child.ReplacedBy))
                                {
                                    // case: replacing a detail on this bom with a new detail
                                    var replacement = current.Children.FirstOrDefault(c => c.ReplacementFor == child.Id);

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

                                    child.DeleteReplaceSeq = replacementSeq + 1;
                                    replacedDetail.DeleteChangeId = change.ChangeId;
                                    replacedDetail.DeleteReplaceSeq = replacementSeq + 1;
                                    replacedDetail.ChangeState = "PROPOS";
                                }
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

        public void CopyBom(string srcPartNumber, string destBomPartNumber, int changedBy, int crfNumber)
        {
            var destBom = this.bomRepository.FindBy(x => x.BomName == destBomPartNumber);
            var change = this.bomChangeRepository.FindBy(
                x => x.DocumentNumber == crfNumber && x.BomId == destBom.BomId && new[] { "PROPOS", "ACCEPT" }.Contains(x.ChangeState));
            
            if (change == null)
            {
                var changeId = this.databaseService.GetIdSequence("CHG_SEQ");
                change = new BomChange
                             {
                                 ChangeId = changeId,
                                 BomName = destBom.BomName,
                                 BomId = destBom.BomId,
                                 DocumentType = "CRF",
                                 DocumentNumber = crfNumber,
                                 DateEntered = DateTime.Today,
                                 EnteredById = changedBy,
                                 ChangeState = "PROPOS",
                                 PartNumber = destBom.BomName,
                                 Comments = "BOM_UT",
                                 PcasChange = "N"
                             };
                this.bomChangeRepository.Add(change);
            }

            this.bomPack.CopyBom(srcPartNumber, destBom.BomId, change.ChangeId, "PROPOS", "O");
        }

        public void DeleteAllFromBom(string bomName, int crfNumber, int changedBy)
        {
            var bom = this.bomRepository.FindBy(x => x.BomName == bomName);
            var change = new BomChange
                             {
                                 BomId = bom.BomId,
                                 ChangeId = this.databaseService.GetIdSequence("CHG_SEQ"),
                                 BomName = bomName,
                                 DocumentType = "CRF", // for now
                                 DocumentNumber = crfNumber,
                                 PartNumber = bomName,
                                 DateEntered = DateTime.Today,
                                 EnteredById = changedBy,
                                 ChangeState = "PROPOS",
                                 Comments = "BOM_UT",
                                 PcasChange = "N"
                             };
            this.bomChangeRepository.Add(change);
            foreach (var child in bom.Details)
            {
                var detail = this.bomDetailRepository.FindById(child.DetailId);
                if (!detail.DeleteChangeId.HasValue && new[] { "PROPOS", "ACCEPT", "LIVE" }.Contains(detail.ChangeState))
                {
                    detail.DeleteChangeId = change.ChangeId;
                }
            }
        }
    }
}
