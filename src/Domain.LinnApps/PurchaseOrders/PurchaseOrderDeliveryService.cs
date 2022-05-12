namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Keys;

    public class PurchaseOrderDeliveryService : IPurchaseOrderDeliveryService
    {
        private readonly IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository;

        private readonly IAuthorisationService authService;

        public PurchaseOrderDeliveryService(
            IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository,
            IAuthorisationService authService)
        {
            this.repository = repository;
            this.authService = authService;
        }

        public IEnumerable<PurchaseOrderDelivery> SearchDeliveries(
            string supplierSearchTerm, string orderNumberSearchTerm, bool includeAcknowledged)
        {
            var result = this.repository.FindAll();
            if (!string.IsNullOrEmpty(supplierSearchTerm))
            {
                if (int.TryParse(supplierSearchTerm, out var supplierId))
                {
                    result = result.Where(x => x.PurchaseOrderDetail.PurchaseOrder.SupplierId == supplierId);
                }
                else
                {
                    result = result.Where(x => 
                        x.PurchaseOrderDetail.PurchaseOrder.Supplier.Name.Contains(supplierSearchTerm.ToUpper()));
                }
            }

            if (!string.IsNullOrEmpty(orderNumberSearchTerm))
            {
                result = result.Where(x => x.OrderNumber.ToString().Contains(orderNumberSearchTerm));
            }

            if (!includeAcknowledged)
            {
                result = result.Where(x => !x.DateAdvised.HasValue);
            }

            return result.OrderBy(x => x.OrderNumber);
        }

        public PurchaseOrderDelivery UpdateDelivery(
            PurchaseOrderDeliveryKey key,
            PurchaseOrderDelivery from,
            PurchaseOrderDelivery to,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to acknowledge orders.");
            }

            var entity = this.repository.FindById(key);

            if (from.DateAdvised != to.DateAdvised)
            {
                entity.DateAdvised = to.DateAdvised;
            }

            if (from.RescheduleReason != to.RescheduleReason)
            {
                entity.RescheduleReason = to.RescheduleReason;
            }

            if (from.SupplierConfirmationComment != to.SupplierConfirmationComment)
            {
                entity.SupplierConfirmationComment = to.SupplierConfirmationComment;
            }

            if (from.AvailableAtSupplier != to.AvailableAtSupplier)
            {
                entity.AvailableAtSupplier = to.AvailableAtSupplier;
            }

            return entity;
        }

        public BatchUpdateProcessResult BatchUpdateDeliveries(
            IEnumerable<PurchaseOrderDeliveryUpdate> changes, IEnumerable<string> privileges)
        {
            throw new System.NotImplementedException();
        }
    }
}
