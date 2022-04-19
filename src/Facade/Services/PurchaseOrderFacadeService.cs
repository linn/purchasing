﻿namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    public class
        PurchaseOrderFacadeService : FacadeResourceService<PurchaseOrder, int, PurchaseOrderResource,
            PurchaseOrderResource>
    {
        private readonly IPurchaseOrderService domainService;

        private readonly IRepository<OverbookAllowedByLog, int> overbookAllowedByLogRepository;

        public PurchaseOrderFacadeService(
            IRepository<PurchaseOrder, int> repository,
            ITransactionManager transactionManager,
            IBuilder<PurchaseOrder> resourceBuilder,
            IPurchaseOrderService domainService,
            IRepository<OverbookAllowedByLog, int> overbookAllowedByLogRepository)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
            this.overbookAllowedByLogRepository = overbookAllowedByLogRepository;
        }

        protected override PurchaseOrder CreateFromResource(
            PurchaseOrderResource resource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(PurchaseOrder entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PurchaseOrder entity,
            PurchaseOrderResource resource,
            PurchaseOrderResource updateResource)
        {
            // if is overbook form
            var log = new OverbookAllowedByLog
                          {
                              OrderNumber = entity.OrderNumber,
                              OverbookQty = entity.OverbookQty,
                              OverbookDate = DateTime.Now,
                              OverbookGrantedBy = userNumber
                          };
            this.overbookAllowedByLogRepository.Add(log);
        }

        protected override Expression<Func<PurchaseOrder, bool>> SearchExpression(string searchTerm)
        {
            return x => x.OrderNumber.ToString().Contains(searchTerm);
        }

        protected override void UpdateFromResource(
            PurchaseOrder entity,
            PurchaseOrderResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var updated = this.BuildEntityFromResourceHelper(updateResource);

            //check if overbook form or not, direct to different domain methods
            this.domainService.AllowOverbook(entity, updated, privileges);
        }

        private PurchaseOrder BuildEntityFromResourceHelper(PurchaseOrderResource resource)
        {
            return new PurchaseOrder
                       {
                           OrderNumber = resource.OrderNumber,
                           Cancelled = resource.Cancelled,
                           DocumentTypeName = resource.DocumentType,
                           OrderDate = resource.DateOfOrder,
                           Overbook = resource.Overbook,
                           OverbookQty = resource.OverbookQty,
                           SupplierId = resource.SupplierId
                       };
        }
    }
}
