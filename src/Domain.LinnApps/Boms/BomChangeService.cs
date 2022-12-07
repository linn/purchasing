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

        public BomChange CreateBomChange(BomTreeNode tree, int changeRequestNumber, int enteredBy)
        {
            
            // add a new bom_change - db triggers will create the bom and associate with the part
            var id = this.databaseService.GetIdSequence("CHG_SEQ");
            var change = new BomChange
                                 {
                                     BomId = id,
                                     BomName = tree.Name,
                                     DocumentType = "CRF", // for now
                                     DocumentNumber = changeRequestNumber,
                                     PartNumber = tree.Name,
                                     DateEntered = DateTime.Today,
                                     EnteredBy = enteredBy,
                                     ChangeState = "PROPOS",
                                     Comments = "BOM_UT",
                                     PcasChange = "N"
                                 };
            this.bomChangeRepository.Add(change);

            return change;
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
