namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderReqStateFacadeService : FacadeResourceService<PurchaseOrderReqState, string, PurchaseOrderReqStateResource, PurchaseOrderReqStateResource>
    {
        public PurchaseOrderReqStateFacadeService(
            IRepository<PurchaseOrderReqState, string> repository,
            ITransactionManager transactionManager,
            IBuilder<PurchaseOrderReqState> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override PurchaseOrderReqState CreateFromResource(
            PurchaseOrderReqStateResource resource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            PurchaseOrderReqState entity,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PurchaseOrderReqState entity,
            PurchaseOrderReqStateResource resource,
            PurchaseOrderReqStateResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PurchaseOrderReqState, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            PurchaseOrderReqState entity,
            PurchaseOrderReqStateResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
