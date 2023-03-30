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

        public void ProcessTreeUpdate(BomTreeNode tree, int changeRequestNumber, int enteredBy)
        {
            // traversing the updated tree to enact changes to all boms involved in this change...
            var q = new Queue<BomTreeNode>();
            q.Enqueue(tree);
            while (q.Count != 0)
            {
                var n = q.Count;

                while (n > 0)
                {
                    var current = q.Dequeue();

                    if (current.AssemblyHasChanges.GetValueOrDefault() && current.Children != null)
                    {
                        var bom = this.GetOrCreateBom(current.Name);
                        var change = this.GetOrCreateBomChange(current.Name, changeRequestNumber, enteredBy, bom);
                        this.ProcessBomChange(current, change, bom);
                        current.AssemblyHasChanges = false;
                    }

                    if (current.Children != null)
                    {
                        for (var i = 0; i < current.Children.Count(); i++)
                        {
                            if (current.Children.Any())
                            {
                                q.Enqueue(current.Children.ElementAt(i));
                            }
                        }
                    }
                    
                    n--;
                }
            }
        }

        public void CopyBom(
            string srcPartNumber, string destBomPartNumber, int changedBy, int crfNumber, string addOrOverwrite)
        {
            var change = this.GetOrCreateBomChange(
                destBomPartNumber, crfNumber, changedBy, this.GetOrCreateBom(destBomPartNumber));
            this.bomPack.CopyBom(srcPartNumber, change.BomId, change.ChangeId, change.ChangeState, addOrOverwrite);
        }

        public void DeleteAllFromBom(string bomName, int crfNumber, int changedBy)
        {
            var bom = this.bomRepository.FindBy(b => b.BomName == bomName);
            var change = this.GetOrCreateBomChange(bomName, crfNumber, changedBy, bom);
            foreach (var child in bom.Details)
            {
                var detail = this.bomDetailRepository.FindById(child.DetailId);
                if (!detail.DeleteChangeId.HasValue && new[] { "PROPOS", "ACCEPT", "LIVE" }
                        .Contains(detail.ChangeState))
                {
                    detail.DeleteChangeId = change.ChangeId;
                    detail.ChangeState = "HIST";
                }
            }
        }

        public void ExplodeSubAssembly(string bomName, int crfNumber, string subAssembly, int changedBy)
        {
            var change = this.GetOrCreateBomChange(
                bomName, crfNumber, changedBy, this.GetOrCreateBom(bomName));
            this.bomPack.ExplodeSubAssembly(change.BomId, change.ChangeId, change.ChangeState, subAssembly);
        }

        public void ReplaceBomDetail(int detailId, ChangeRequest request, int changedBy, decimal? newQty)
        {
            if (request == null)
            {
                throw new ItemNotFoundException("Change Request not found");
            }

            var detail = this.bomDetailRepository.FindById(detailId);
            if (detail == null)
            {
                throw new ItemNotFoundException("Bom Detail not found");
            }

            // check something not already deleting this change request
            if (detail.DeleteChange != null)
            {
                return;
            }

            if (detail.PcasLine == "Y")
            {
                // TODO work out how we cope with doing the PCAS changes maybe call pcas pack
                return;
            }

            // check that change request is setup to do this
            if (!request.CanReplace(true))
            {
                return;
            }

            // see if already a change for that detail's bom id
            var change = request.BomChanges?.FirstOrDefault(c => c.BomId == detail.BomId & (c.ChangeState == "ACCEPT" || c.ChangeState == "PROPOS"));
            if (change == null)
            {
                var bom = this.bomRepository.FindById(detail.BomId);

                change = new BomChange
                             {
                                 BomId = detail.BomId,
                                 ChangeId = this.databaseService.GetIdSequence("CHG_SEQ"),
                                 BomName = bom.BomName,
                                 DocumentType = request.DocumentType,
                                 DocumentNumber = request.DocumentNumber,
                                 PartNumber = bom.Part.PartNumber,
                                 DateEntered = DateTime.Today,
                                 EnteredById = changedBy,
                                 ChangeState = request.ChangeState,
                                 Comments = "Replace",
                                 PcasChange = detail.PcasLine
                             };
                this.bomChangeRepository.Add(change);
            }

            var otherReplaceSeq = change.AddedBomDetails?.Max(d => d.AddReplaceSeq);

            // set bom detail to be deleted by this change 
            detail.DeleteChange = change;
            detail.DeleteChangeId = change.ChangeId;
            detail.DeleteReplaceSeq = otherReplaceSeq == null ? 1 : (int)otherReplaceSeq + 1;

            // now add the second half
            if ((newQty == null) || (newQty > 0))
            {
                var id = this.databaseService.GetIdSequence("BOMDET_SEQ");
                this.bomDetailRepository.Add(new BomDetail
                                                 {
                                                     DetailId = id,
                                                     BomId = change.BomId,
                                                     PartNumber = request.NewPart.PartNumber,
                                                     Qty = newQty ?? detail.Qty,
                                                     GenerateRequirement = detail.GenerateRequirement,
                                                     ChangeState = request.ChangeState,
                                                     AddChangeId = change.ChangeId,
                                                     AddReplaceSeq = otherReplaceSeq == null ? 1 : (int)otherReplaceSeq + 1,
                                                     DeleteChangeId = null,
                                                     DeleteReplaceSeq = null,
                                                     PcasLine = detail.PcasLine
                                                 });
            }
        }

        public void AddBomDetail(string bomName, ChangeRequest request, int changedBy, decimal? newQty)
        {
            if (request == null)
            {
                throw new ItemNotFoundException("Change Request not found");
            }

            // check if we're even allowed to do this to this change request
            if (!request.CanReplace(true))
            {
                return;
            }

            // see if already a change for that bom
            var change = request.BomChanges?.FirstOrDefault(c => c.BomName == bomName & (c.ChangeState == "ACCEPT" || c.ChangeState == "PROPOS"));
            if (change == null)
            {
                var bom = this.bomRepository.FindBy(b => b.BomName == bomName);
                if (bom == null)
                {
                    throw new ItemNotFoundException("Bom not found");
                }


                change = new BomChange
                             {
                                 BomId = bom.BomId,
                                 ChangeId = this.databaseService.GetIdSequence("CHG_SEQ"),
                                 BomName = bom.BomName,
                                 DocumentType = request.DocumentType,
                                 DocumentNumber = request.DocumentNumber,
                                 PartNumber = bom.Part.PartNumber,
                                 DateEntered = DateTime.Today,
                                 EnteredById = changedBy,
                                 ChangeState = request.ChangeState,
                                 Comments = "Replace Add",
                                 PcasChange = "N"
                             };
                this.bomChangeRepository.Add(change);
            }

            // now add the second half
            if (newQty > 0)
            {
                var id = this.databaseService.GetIdSequence("BOMDET_SEQ");
                this.bomDetailRepository.Add(new BomDetail
                                                 {
                                                     DetailId = id,
                                                     BomId = change.BomId,
                                                     PartNumber = request.NewPart.PartNumber,
                                                     Qty = newQty,
                                                     GenerateRequirement = "Y",
                                                     ChangeState = request.ChangeState,
                                                     AddChangeId = change.ChangeId,
                                                     DeleteChangeId = null,
                                                     DeleteReplaceSeq = null,
                                                     PcasLine = "N"
                                                 });
            }
        }

        public void ReplaceAllBomDetails(ChangeRequest request, int changedBy, decimal? newQty)
        {
            var details = this.bomDetailRepository.FilterBy(
                b => b.PartNumber == request.OldPartNumber && b.ChangeState != "HIST" && b.DeleteChangeId == null);
            foreach (var detail in details)
            {
                this.ReplaceBomDetail(detail.DetailId, request, changedBy, newQty);
            }
        }

        private void ProcessBomChange(BomTreeNode current, BomChange change, Bom bom)
        {
            var detailsOnChange = this.bomDetailRepository
                .FilterBy(x => x.BomId == change.BomId && x.AddChangeId == change.ChangeId);

            var replacementSeq = detailsOnChange == null || !detailsOnChange.Any() ? 0
                                     : detailsOnChange.Max(d => d.AddReplaceSeq.GetValueOrDefault());

            foreach (var child in current.Children)
            {
                var isAddition = bom.Details
                                 == null || bom.Details.Count == 0
                                         || bom.Details.All(d => d.DetailId.ToString() != child.Id);

                var isDeletion = child.ToDelete.GetValueOrDefault();

                var isDeletionUndo = !child.IsReplaced && child.ChangeState == "LIVE"
                                            && child.DeleteChangeDocumentNumber.HasValue;

                var isExistingNode = bom.Details != null && string.IsNullOrEmpty(child.ReplacedBy)
                                                    && bom.Details.Select(x => x.DetailId.ToString())
                                                        .Contains(child.Id);

                var isReplacement = !string.IsNullOrEmpty(child.ReplacedBy);

                if (isDeletionUndo)
                {
                    this.UndoNodeDeletion(child, change.DocumentNumber);
                }
                else if (isDeletion)
                {
                    this.DeleteNode(child, change);
                }
                else
                {
                    if (isAddition)
                    {
                        this.AddNode(child, change, ref replacementSeq);
                    }
                    else if (isExistingNode && child.HasChanged.GetValueOrDefault())
                    {
                        this.MaybeUpdateNode(child, change);
                    }

                    if (isReplacement)
                    {
                        var replacement = current.Children.FirstOrDefault(c => c.ReplacementFor == child.Id);
                        this.DoNodeReplacement(child, replacement, change, replacementSeq);
                    }
                }
            }
        }

        private void AddNode(BomTreeNode node, BomChange change, ref int replacementSeq)
        {
            this.CheckPart(node.Name, node.ParentName);

            if (node.Name == change.BomName)
            {
                throw new InvalidBomChangeException($"Can't add {node.Name} to it's own BOM!");
            }

            var id = this.databaseService.GetIdSequence("BOMDET_SEQ");

            this.bomDetailRepository.Add(new BomDetail
            {
                DetailId = id,
                BomId = change.BomId,
                PartNumber = node.Name,
                Qty = node.Qty,
                GenerateRequirement = node.Requirement,
                ChangeState = change.ChangeState,
                AddChangeId = change.ChangeId,
                AddReplaceSeq = string.IsNullOrEmpty(node.ReplacementFor)
                                                     ? null : replacementSeq += 1,
                DeleteChangeId = null,
                DeleteReplaceSeq = null,
                PcasLine = "N"
            });
        }

        private void MaybeUpdateNode(BomTreeNode node, BomChange change)
        {
            var detail = this.bomDetailRepository.FindById(int.Parse(node.Id));

            if (detail.AddChange.DocumentNumber != change.DocumentNumber)
            {
                throw new InvalidBomChangeException(
                    "Can't directly update details added by a different CRF - Replace them instead!");
            }

            if (detail.PartNumber != node.Name)
            {
                this.CheckPart(node.Name, node.ParentName);
                detail.PartNumber = node.Name;
            }

            if (detail.Qty != node.Qty || detail.GenerateRequirement != node.Requirement)
            {
                detail.GenerateRequirement = node.Requirement;
                detail.Qty = node.Qty;
            }
        }

        private void DeleteNode(BomTreeNode node, BomChange change)
        {
            var detail = this.bomDetailRepository.FindById(int.Parse(node.Id));
            if (detail.DeleteChangeId.HasValue
                && detail.DeleteChange?.DocumentNumber != change.DocumentNumber)
            {
                throw new InvalidBomChangeException(
                    $"{node.Name} is already marked for deletion by another change request"
                    + $" ({detail.DeleteChange?.DocumentNumber})");
            }

            if (detail.PcasLine == "Y")
            {
                throw new InvalidBomChangeException($"{node.Name} is a PCAS line - cannot delete here.");
            }

            if (detail.AddChange.DocumentNumber == change.DocumentNumber)
            {
                this.bomDetailRepository.Remove(detail);

                if (detail.AddReplaceSeq.HasValue)
                {
                    var replacedDetail = this.bomDetailRepository.FindBy(
                        x => x.DeleteChangeId == change.ChangeId
                             && x.DeleteReplaceSeq == detail.AddReplaceSeq);
                    replacedDetail.DeleteReplaceSeq = null;
                    replacedDetail.DeleteChangeId = null;
                }
            }
            else
            {
                detail.DeleteChangeId = change.ChangeId;
            }
        }

        private void UndoNodeDeletion(BomTreeNode node, int changeRequestNumber)
        {
            // todo - is this correct? - if so add a unit test to enshrine it
            if (node.DeleteChangeDocumentNumber.GetValueOrDefault() != changeRequestNumber)
            {
                throw new InvalidBomChangeException(
                    $"cannot undo delete of {node.Name} - it was deleted on a different change request.");
            }

            node.DeleteChangeDocumentNumber = null;
            node.ChangeState = "LIVE"; // todo - check this is right - add to test if so
            this.bomDetailRepository.FindById(int.Parse(node.Id)).DeleteChangeId = null;
        }

        private void DoNodeReplacement(BomTreeNode node, BomTreeNode replacement, BomChange change, int seq)
        {
            if (replacement == null)
            {
                throw new InvalidBomChangeException(
                    $"{node.Name} is marked for replacement but no replacement part is specified");
            }

            var replacedDetail = this.bomDetailRepository.FindById(int.Parse(node.Id));

            if (node.AddChangeDocumentNumber == change.DocumentNumber)
            {
                throw new InvalidBomChangeException(
                    $"{node.Name} was added by the current change request - no need to replace it - just edit it directly.");
            }

            if (replacedDetail.PcasLine == "Y")
            {
                throw new InvalidBomChangeException(
                    $"{node.Name} is a PCAS line - cannot replace here.");
            }

            replacedDetail.DeleteChangeId = change.ChangeId;
            replacedDetail.DeleteReplaceSeq = seq + 1;
        }

        private Bom GetOrCreateBom(string name)
        {
            var bomLookup = this.bomRepository.FindBy(x => x.BomName == name);
            var bom = bomLookup ?? new Bom
                                       {
                                           BomId = this.databaseService.GetIdSequence("BOM_SEQ"),
                                           BomName = name,
                                           Part = this.partRepository.FindBy(x => x.PartNumber == name),
                                           Depth = 1,
                                           CommonBom = "N"
                                       };

            if (bomLookup == null)
            {
                this.bomRepository.Add(bom);
                bom.Part.BomId = bom.BomId;
            }

            return bom;
        }

        private BomChange GetOrCreateBomChange(string bomName, int crfNumber, int changedBy, Bom bom)
        {
            var change = this.bomChangeRepository.FilterBy(
                x => x.DocumentNumber == crfNumber
                     && x.BomId == bom.BomId
                     && new[] { "PROPOS", "ACCEPT" }.Contains(x.ChangeState))?.FirstOrDefault();
            if (change == null)
            {
                change = new BomChange
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
            }

            return change;
        }

        private void CheckPart(string partNumber, string assembly)
        {
            var part = this.partRepository.FindBy(x => x.PartNumber == partNumber);
            if (part == null)
            {
                throw new ItemNotFoundException($"Invalid Part Number: {partNumber} on Assembly: {assembly}");
            }

            if (part.DatePurchPhasedOut.HasValue)
            {
                throw new InvalidBomChangeException(
                $"Can't add {partNumber} to {assembly} - part has been phased out by purchasing");
            }

            if (string.IsNullOrEmpty(part.DecrementRule))
            {
                throw new InvalidBomChangeException(
                $"Can't add {partNumber} to {assembly} - part has no decrement rule!");
            }

            if (string.IsNullOrEmpty(part.BomType))
            {
                throw new InvalidBomChangeException(
                    $"Can't add {partNumber} to {assembly} - part has no BOM Type!");
            }
        }
    }
}
