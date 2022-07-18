namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.Facade.Extensions;
    using Linn.Purchasing.Resources;

    public class AutomaticPurchaseOrderFacadeService : FacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource>
    {
        private readonly IAutomaticPurchaseOrderService automaticPurchaseOrderService;

        public AutomaticPurchaseOrderFacadeService(
            IRepository<AutomaticPurchaseOrder, int> repository,
            ITransactionManager transactionManager,
            IBuilder<AutomaticPurchaseOrder> resourceBuilder,
            IAutomaticPurchaseOrderService automaticPurchaseOrderService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.automaticPurchaseOrderService = automaticPurchaseOrderService;
        }

        protected override AutomaticPurchaseOrder CreateFromResource(AutomaticPurchaseOrderResource resource, IEnumerable<string> privileges = null)
        {
            return this.automaticPurchaseOrderService.CreateAutomaticPurchaseOrder(resource.ToDomain());
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
