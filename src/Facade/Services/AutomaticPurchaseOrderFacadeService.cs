namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class AutomaticPurchaseOrderFacadeService : FacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource>
    {
        public AutomaticPurchaseOrderFacadeService(
            IRepository<AutomaticPurchaseOrder, int> repository,
            ITransactionManager transactionManager,
            IBuilder<AutomaticPurchaseOrder> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override AutomaticPurchaseOrder CreateFromResource(AutomaticPurchaseOrderResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            AutomaticPurchaseOrder entity,
            AutomaticPurchaseOrderResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<AutomaticPurchaseOrder, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            AutomaticPurchaseOrder entity,
            AutomaticPurchaseOrderResource resource,
            AutomaticPurchaseOrderResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(AutomaticPurchaseOrder entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
