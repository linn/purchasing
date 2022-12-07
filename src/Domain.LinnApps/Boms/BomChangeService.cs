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

        public BomChangeService(
            IDatabaseService databaseService, 
            IQueryRepository<Part> partsRepository,
            IRepository<BomChange, int> bomChangeRepository,
            IBomDetailRepository bomDetailRepository)
        {
            this.databaseService = databaseService;
            this.bomChangeRepository = bomChangeRepository;
            this.bomDetailRepository = bomDetailRepository;
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

                    var id = this.databaseService.GetIdSequence("CHG_SEQ");
                    var change = new BomChange
                                     {
                                         BomId = id,
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

                    // now we need to update the current assembly's details to match the new list
                    // annoyingly they need to reference the bom_change which might not exist yet :(
                    // could maybe just add the details with a null changeId, and then use the return of this function to populate the change ids after the change add has been committed

                    // for every detail part...
                    // general approach will be adding a new detail if this subassembly doesn't have that part on the bom
                    // or updating the qty if this sub assembly does have this part on the bom
                    // can worry about deleting stuff later
                    for (var i = 0; i < current.Children.Count(); i++)
                    {
                        // only queue subtrees with changes for processing
                        if (current.Type != "C" && current.HasChanged.GetValueOrDefault())
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
