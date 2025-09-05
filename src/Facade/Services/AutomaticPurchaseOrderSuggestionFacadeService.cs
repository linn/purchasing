namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public class AutomaticPurchaseOrderSuggestionFacadeService : FacadeFilterResourceService<AutomaticPurchaseOrderSuggestion, int, AutomaticPurchaseOrderSuggestionResource, AutomaticPurchaseOrderSuggestionResource, PlannerSupplierRequestResource>
    {
        public AutomaticPurchaseOrderSuggestionFacadeService(IRepository<AutomaticPurchaseOrderSuggestion, int> repository, ITransactionManager transactionManager, IBuilder<AutomaticPurchaseOrderSuggestion> builder)
            : base(repository, transactionManager, builder)
        {
        }

        protected override AutomaticPurchaseOrderSuggestion CreateFromResource(
            AutomaticPurchaseOrderSuggestionResource resource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            AutomaticPurchaseOrderSuggestion entity,
            AutomaticPurchaseOrderSuggestionResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<AutomaticPurchaseOrderSuggestion, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            AutomaticPurchaseOrderSuggestion entity,
            AutomaticPurchaseOrderSuggestionResource resource,
            AutomaticPurchaseOrderSuggestionResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(AutomaticPurchaseOrderSuggestion entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<AutomaticPurchaseOrderSuggestion, bool>> FilterExpression(PlannerSupplierRequestResource searchResource)
        {
            if (searchResource.Planner.HasValue && searchResource.SupplierId.HasValue)
            {
                return a => a.PreferredSupplierId == searchResource.SupplierId && a.Planner == searchResource.Planner && a.RecommendedQuantity > 1;
            }

            if (searchResource.Planner.HasValue)
            {
                return a => a.Planner == searchResource.Planner && a.RecommendedQuantity > 1;
            }

            if (searchResource.SupplierId.HasValue)
            {
                return a => a.PreferredSupplierId == searchResource.SupplierId && a.RecommendedQuantity > 1;
            }

            return a => true;
        }

        protected override Expression<Func<AutomaticPurchaseOrderSuggestion, bool>> FindExpression(PlannerSupplierRequestResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
