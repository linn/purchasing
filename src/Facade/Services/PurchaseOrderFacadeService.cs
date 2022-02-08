namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    public class PurchaseOrderFacadeService
        : FacadeFilterResourceService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource, PurchaseOrderSearchResource>
    {
        private readonly IPurchaseOrderService domainService;

        private readonly IBuilder<ResultsModel> resultsModelResourceBuilder;


        public PurchaseOrderFacadeService(
            IRepository<PurchaseOrder, int> repository,
            ITransactionManager transactionManager,
            IBuilder<PurchaseOrder> resourceBuilder,
            IPurchaseOrderService domainService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
        }

        protected override PurchaseOrder CreateFromResource(PurchaseOrderResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(string actionType, int userNumber, PurchaseOrder entity, PurchaseOrderResource resource, PurchaseOrderResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(PurchaseOrder entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(PurchaseOrder entity, PurchaseOrderResource updateResource, IEnumerable<string> privileges = null)
        {
            var updated = this.BuildEntityFromResourceHelper(updateResource);

            updated.Overbook = entity.Overbook;
            updated.OverbookQty = entity.OverbookQty;

            this.domainService.UpdatePurchaseOrder(entity, updated, updateResource.Privileges);
        }

        protected override Expression<Func<PurchaseOrder, bool>> SearchExpression(string searchTerm)
        {
            return x => x.OrderNumber.ToString().Contains(searchTerm);
        }

        protected override Expression<Func<PurchaseOrder, bool>> FilterExpression(PurchaseOrderSearchResource searchResource)
        {
            throw new NotImplementedException();
        }

        private PurchaseOrder BuildEntityFromResourceHelper(PurchaseOrderResource resource)
        {
            return new PurchaseOrder
            {
                OrderNumber = resource.OrderNumber,
                Cancelled = resource.Cancelled,
                DocumentType = resource.DocumentType,
                OrderDate = resource.DateOfOrder,
                Overbook = resource.Overbook,
                OverbookQty = resource.OverbookQty,
                SupplierId = resource.SupplierId,
            };
        }
    }
}
