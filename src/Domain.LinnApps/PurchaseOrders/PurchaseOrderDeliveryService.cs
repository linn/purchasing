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
            string supplierSearchTerm,
            string orderNumberSearchTerm,
            bool includeAcknowledged)
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
                    result = result.Where(
                        x => x.PurchaseOrderDetail.PurchaseOrder.Supplier.Name.Contains(supplierSearchTerm.ToUpper()));
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
            IEnumerable<PurchaseOrderDeliveryUpdate> changes,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to acknowledge orders.");
            }

            var successCount = 0;

            var errors = new List<Error>();

            var purchaseOrderDeliveryUpdates = changes as PurchaseOrderDeliveryUpdate[] ?? changes.ToArray();
            foreach (var change in purchaseOrderDeliveryUpdates)
            {
                var entity = this.repository.FindById(change.Key);
                if (string.IsNullOrEmpty(change.NewReason))
                {
                    change.NewReason = "ADVISED";
                }

                if (entity == null)
                {
                    errors.Add(
                        new Error(
                            $"{change.Key.OrderNumber} / {change.Key.OrderLine} / {change.Key.DeliverySequence}",
                            "Could not find a delivery corresponding to the above ORDER / LINE / DELIVERY NO."));
                }
                else if (this.repository.FilterBy(
                             x => x.OrderNumber == change.Key.OrderNumber).Count() > 1
                         || purchaseOrderDeliveryUpdates.Count(c => c.Key.OrderNumber == change.Key.OrderNumber) > 1
                         || change.Key.DeliverySequence > 1)
                {
                    errors.Add(
                        new Error(
                            $"{change.Key.OrderNumber} / {change.Key.OrderLine} / {change.Key.DeliverySequence}",
                            $"{change.Key.OrderNumber} / {change.Key.OrderLine} / {change.Key.DeliverySequence} has been split over multiple deliveries. Please acknowledge manually."));
                }
                else if (change.NewReason == "")
                {
                    // check valid change reason
                }
                else
                {
                    entity.DateAdvised = change.NewDateAdvised;
                    entity.RescheduleReason = change.NewReason;
                    successCount++;
                }
            }

            if (errors.Any())
            {
                return new BatchUpdateProcessResult
                           {
                               Success = false,
                               Message =
                                   $"{successCount} records updated successfully. The following errors occurred: ",
                               Errors = errors
                           };
            }

            return new BatchUpdateProcessResult
                       {
                           Success = true, Message = $"{successCount} records updated successfully."
                       };
        }
    }
}

