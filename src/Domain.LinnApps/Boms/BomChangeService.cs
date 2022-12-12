namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class BomChangeService : IBomChangeService
    {
        private readonly IDatabaseService databaseService;

        private readonly IRepository<BomChange, int> bomChangeRepository;

        private readonly IBomDetailRepository bomDetailRepository;

        private readonly IRepository<Bom, int> bomRepository;

        public BomChangeService(
            IDatabaseService databaseService, 
            IQueryRepository<Part> partsRepository,
            IRepository<BomChange, int> bomChangeRepository,
            IBomDetailRepository bomDetailRepository,
            IRepository<Bom, int> bomRepository)
        {
            this.databaseService = databaseService;
            this.bomChangeRepository = bomChangeRepository;
            this.bomDetailRepository = bomDetailRepository;
            this.bomRepository = bomRepository;
        }

        public IEnumerable<BomChange> CreateBomChanges(BomTreeNode tree, int changeRequestNumber, int enteredBy)
        {
            var result = new List<BomChange>();

            // add a new bom_change for any bom that has changed - db triggers will create the bom if required
            var q = new Queue<BomTreeNode>();
            q.Enqueue(tree);
            while (q.Count != 0)
            {
                var n = q.Count;

                while (n > 0)
                {
                    var current = q.Dequeue();
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
                                             EnteredBy = enteredBy,
                                             ChangeState = "PROPOS",
                                             Comments = "BOM_UT",
                                             PcasChange = "N"
                                         };
                        this.bomChangeRepository.Add(change);
                        result.Add(change);


                        foreach (var child in current.Children)
                        {
                            if (bom.Details.All(d => d.PartNumber != child.Name))
                            {
                                // bom.Details.Add(new BomDetailViewEntry
                                //                     {
                                //                         BomName = current.Name, 
                                //                         BomId = bom.BomId, 
                                //                         DetailId = 0, // seq
                                //                         PartNumber = child.Name,
                                //                         Qty = child.Qty, // todo - is this passed?
                                //                         // GenerateRequirement = null,
                                //                         ChangeState = "PROPOS",
                                //                         AddChangeId = id,
                                //                         AddReplaceSeq = null, // todo - not supporting replacements yet
                                //                         DeleteChangeId = null, // todo - not supporting deletions yet
                                //                         DeleteReplaceSeq = null, // todo - not supporting deletions yet
                                //                         PartRequirement = null,
                                //                         BomPartNumber = current.Name,
                                //                         BomPart = null,
                                //                         PcasLine = null,
                                //                         Components = null
                                //                     });
                            }
                        }

                        // now we need to update the current assembly's details to match the new list
                        // annoyingly they need to reference the bom_change which might not exist yet :(
                        // could add the details with a null changeId, and then use the return of this function to populate the change ids after the new bom_change's committed?

                        // general approach:
                        // for every detail part...
                        // add a new detail if the current subassembly doesn't have that part on the bom
                        // or update the qty/reqt if this sub assembly does have this part on the bom
                        // can worry about deletes later
                    }


                    for (var i = 0; i < current.Children.Count(); i++)
                    {
                        if (current.Type != "C")
                        {
                            q.Enqueue(current.Children.ElementAt(i));
                        }
                    }

                    n--;
                }
            }

            return result;
        }

        public BomTreeNode CreateBom(BomTreeNode tree, int changeRequestNumber, int createdBy)
        {
            // traverse the tree adding all the children to bom_details
            var q = new Queue<BomTreeNode>();
            q.Enqueue(tree);
            while (q.Count != 0)
            {
                var n = q.Count;

                while (n > 0)
                {
                    var current = q.Dequeue();

                    // this.bomDetailRepository.add
                    for (var i = 0; i < current.Children.Count(); i++)
                    {
                        q.Enqueue(current.Children.ElementAt(i));
                    }

                    n--;
                }
            }

            return tree;
        }
    }
}
