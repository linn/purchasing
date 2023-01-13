namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.Boms;

    public class BomFacadeService : IBomFacadeService
    {
        private readonly IBomChangeService bomChangeService;

        private readonly ITransactionManager transactionManager;

        public BomFacadeService(
            IBomChangeService bomChangeService, 
            ITransactionManager transactionManager)
        {
            this.bomChangeService = bomChangeService;
            this.transactionManager = transactionManager;
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

        public IResult<ProcessResultResource> CopyBom(
            string srcPartNumber, string destPartNumber, int changedBy, int crfNumber)
        {
            this.bomChangeService.CopyBom(srcPartNumber, destPartNumber, changedBy, crfNumber);
            this.transactionManager.Commit();

            // todo - some error handling? Could this domain service throw errors?
            return new SuccessResult<ProcessResultResource>(new ProcessResultResource(true, "Copied!"));
        }
    }
}
