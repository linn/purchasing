namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources.Boms;

    public class BomFacadeService : IBomFacadeService
    {
        private readonly IBomChangeService bomChangeService;

        private readonly ITransactionManager transactionManager;

        private readonly IBomTreeService treeService;

        public BomFacadeService(
            IBomChangeService bomChangeService, 
            ITransactionManager transactionManager,
            IBomTreeService treeService)
        {
            this.bomChangeService = bomChangeService;
            this.transactionManager = transactionManager;
            this.treeService = treeService;
        }

        public IResult<BomTreeNode> PostBom(PostBomResource resource)
        {
            try
            {
                var result = this.bomChangeService.CreateBomChanges(
                    resource.TreeRoot,
                    resource.CrNumber,
                    resource.EnteredBy);
                this.transactionManager.Commit();
                return new SuccessResult<BomTreeNode>(result);
            }
            catch (DomainException e)
            {
                return new BadRequestResult<BomTreeNode>(e.Message);
            }
        }

        public IResult<BomTreeNode> CopyBom(string srcPartNumber, int destBomId, string destPartNumber)
        {
            this.bomChangeService.CopyBom(srcPartNumber, destBomId);
            this.transactionManager.Commit();

            var tree = this.treeService.BuildBomTree(destPartNumber, null, false, true);
            return new SuccessResult<BomTreeNode>(tree);
        }
    }
}
