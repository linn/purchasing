namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;

    public class BomTreeReportService : IBomTreeReportsService
    {
        private readonly IRepository<Bom, int> repository;

        private readonly IBomDetailRepository detailRepository;

        public BomTreeReportService(
            IRepository<Bom, int> repository,
            IBomDetailRepository detailRepository)
        {
            this.repository = repository;
            this.detailRepository = detailRepository;
        }

        public IResult<BomTreeNodeResource> GetBomTree(string bomName, int levels)
        {
            var root = this.repository.FindBy(x => x.BomName == bomName);

            var rootNode = new BomTreeNodeResource
                               {
                                   Name = root.BomName,
                                   Id = root.BomId,
                                   Children = root.Details?.Select(
                                       d => new BomTreeNodeResource
                                                {
                                                    Name = d.Part.PartNumber,
                                                    Id = d.DetailId
                                                }).OrderBy(x => x.Name)
                               };

            Queue<BomTreeNodeResource> q = new Queue<BomTreeNodeResource>(); 
            q.Enqueue(rootNode);

            while (q.Count != 0)
            {
                var numChildren = q.Count;

                while (numChildren > 0)
                {
                    var current = q.Dequeue();
                    current.Children = current.Children?.Select(
                        child =>
                            {
                                var node = new BomTreeNodeResource
                                               {
                                                   Name = child.Name,
                                                   Id = child.Id,
                                                   Children = 
                                                       this.detailRepository
                                                           .FilterBy(x => x.BomName == child.Name && x.ChangeState == "LIVE")
                                                           .OrderBy(x => x.Part.PartNumber)
                                                           .Select(
                                                               detail =>
                                                                   
                                                                   new BomTreeNodeResource
                                                                       {
                                                                           Id = detail.DetailId,
                                                                           Name = detail.Part.PartNumber
                                                                       })
                                               };
                                return node;
                            }).ToList();

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

            return new SuccessResult<BomTreeNodeResource>(rootNode);
        }
    }
}
