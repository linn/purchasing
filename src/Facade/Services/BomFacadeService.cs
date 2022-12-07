namespace Linn.Purchasing.Facade.Services
{
    using System;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;   
    using Linn.Purchasing.Resources.Boms;

    public class BomFacadeService : IBomFacadeService
    {
        private readonly IBomChangeService bomChangeService;

        private readonly ITransactionManager transactionManager;

        public BomFacadeService(
            IBomChangeService bomChangeService, ITransactionManager transactionManager)
        {
            this.bomChangeService = bomChangeService;
            this.transactionManager = transactionManager;
        }

        public IResult<BomTreeNode> PostBom(PostBomResource resource)
        {
            var change = this.bomChangeService.CreateBomChange(
                resource.TreeRoot,
                resource.CrNumber.GetValueOrDefault(),
                resource.EnteredBy.GetValueOrDefault());
            this.transactionManager.Commit();
            var result = this.bomChangeService.CreateBom(
                resource.TreeRoot, resource.CrNumber.GetValueOrDefault(), change.ChangeId);
            this.transactionManager.Commit();
            return new SuccessResult<BomTreeNode>(result);
        }
    }
}
