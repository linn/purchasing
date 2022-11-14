namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class BomTreeService : IBomTreeService
    {
        private readonly IRepository<Bom, int> repository;

        private readonly IBomDetailRepository detailRepository;

        public BomTreeService(
            IRepository<Bom, int> repository,
            IBomDetailRepository detailRepository)
        {
            this.repository = repository;
            this.detailRepository = detailRepository;
        }

        public BomTreeNode BuildTree(string bomName, int? levels = null)
        {
            // given with the root node, we need to make repository method calls until this entire bom tree is populated with information

            // basically level order traversal of a general tree
            // where each time we visit a node we populate its children's children by performing an additional .FilterBy() lookup
            // since the children are .Include()'d in the repository methods

            // look up the root node
            var root = this.repository.FindBy(x => x.BomName == bomName);
            var rootNode = new BomTreeNode
                               {
                                   Name = root.BomName,
                                   Children = root.Details?.Select(
                                       d => new BomTreeNode
                                                {
                                                    Name = d.Part.PartNumber,
                                                }).OrderBy(x => x.Name)
                               };

            // queue the root node
            var q = new Queue<BomTreeNode>();
            q.Enqueue(rootNode);

            // while there are nodes in the queue
            while (q.Count != 0)
            {
                // while this node has children
                var numChildren = q.Count;
                while (numChildren > 0)
                {
                    // dequeue a child node
                    var current = q.Dequeue();

                    // populate it's children's children nodes by performing an repository lookup
                    current.Children = current.Children?.Select(
                        child =>
                        {
                            var node = new BomTreeNode
                            {
                                Name = child.Name,
                                Children =
                                    this.detailRepository
                                        .FilterBy(x => x.BomName == child.Name && x.ChangeState == "LIVE")
                                        .OrderBy(x => x.Part.PartNumber)
                                        .Select(
                                            detail =>

                                                new BomTreeNode
                                                    {
                                                        Name = detail.Part.PartNumber
                                                    })
                            };
                            return node;
                        }).ToList();

                    // add all the children (now with their own children populated) to the queue, if they exist
                    if (current.Children != null)
                    {
                        foreach (var child in current.Children)
                        {
                            q.Enqueue(child);
                        }
                    }

                    numChildren--;
                }
            }


            // return the root node - all of its children will have children now, and so on...
            return rootNode;
        }
    }
}
