namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public class PurchaseOrderDeliveryFacadeService : IPurchaseOrderDeliveryFacadeService
    {
        private readonly IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository;

        private readonly IBuilder<PurchaseOrderDelivery> resourceBuilder;

        private readonly ITransactionManager transactionManager;

        private readonly IPurchaseOrderDeliveryService domainService;

        public PurchaseOrderDeliveryFacadeService(
            IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository,
            IBuilder<PurchaseOrderDelivery> resourceBuilder,
            IPurchaseOrderDeliveryService domainService,
            ITransactionManager transactionManager)
        {
            this.repository = repository;
            this.resourceBuilder = resourceBuilder;
            this.domainService = domainService;
            this.transactionManager = transactionManager;
        }

        public IResult<IEnumerable<PurchaseOrderDeliveryResource>> SearchDeliveries(
            string supplierSearchTerm, string orderNumberSearchTerm, bool includeAcknowledged)
        {
            var results = this.domainService.SearchDeliveries(
                supplierSearchTerm,
                orderNumberSearchTerm,
                includeAcknowledged);
            return new SuccessResult<IEnumerable<PurchaseOrderDeliveryResource>>(
                results.Select(x => (PurchaseOrderDeliveryResource)this.resourceBuilder.Build(x, null)));
        }

        public IResult<PurchaseOrderDeliveryResource> PatchDelivery(
            PurchaseOrderDeliveryKey key,
            PatchRequestResource<PurchaseOrderDeliveryResource> requestResource, 
            IEnumerable<string> privileges)
        {
            var privilegesList = privileges.ToList();
            var entity = this.repository.FindById(key);

            this.domainService.UpdateDelivery(
                key,
                BuildEntityFromResourceHelper(requestResource.From),
                BuildEntityFromResourceHelper(requestResource.To),
                privilegesList);

            this.transactionManager.Commit();

            return new SuccessResult<PurchaseOrderDeliveryResource>(
                (PurchaseOrderDeliveryResource)this.resourceBuilder.Build(entity, privilegesList));
        }

        public IResult<ProcessResult> BatchUpdateDeliveries(string csvString, IEnumerable<string> privileges)
        {
            throw new System.NotImplementedException();
            this.transactionManager.Commit();
        }

        private static PurchaseOrderDelivery BuildEntityFromResourceHelper(PurchaseOrderDeliveryResource resource)
        {
            return new PurchaseOrderDelivery
                       {
                           DateAdvised = string.IsNullOrEmpty(resource.DateAdvised) 
                                             ? null : DateTime.Parse(resource.DateAdvised),
                           AvailableAtSupplier = resource.AvailableAtSupplier,
                           RescheduleReason = resource.RescheduleReason,
                           SupplierConfirmationComment = resource.SupplierConfirmationComment
                       };
        }
    }
}
