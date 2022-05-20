namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;

    public class PurchaseOrderDeliveryService : IPurchaseOrderDeliveryService
    {
        private readonly IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository;

        private readonly IAuthorisationService authService;

        private readonly IRepository<RescheduleReason, string> rescheduleReasonRepository;

        private readonly ISingleRecordRepository<PurchaseLedgerMaster> purchaseLedgerMaster;

        private readonly IRepository<MiniOrder, int> miniOrderRepository;

        private readonly IRepository<MiniOrderDelivery, MiniOrderDeliveryKey> miniOrderDeliveryRepository;

        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        public PurchaseOrderDeliveryService(
            IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository,
            IAuthorisationService authService,
            IRepository<RescheduleReason, string> rescheduleReasonRepository,
            ISingleRecordRepository<PurchaseLedgerMaster> purchaseLedgerMaster,
            IRepository<MiniOrder, int> miniOrderRepository,
            IRepository<MiniOrderDelivery, MiniOrderDeliveryKey> miniOrderDeliveryRepository,
            IRepository<PurchaseOrder, int> purchaseOrderRepository)
        {
            this.repository = repository;
            this.authService = authService;
            this.rescheduleReasonRepository = rescheduleReasonRepository;
            this.purchaseLedgerMaster = purchaseLedgerMaster;
            this.miniOrderRepository = miniOrderRepository;
            this.miniOrderDeliveryRepository = miniOrderDeliveryRepository;
            this.purchaseOrderRepository = purchaseOrderRepository;
        }

        public IEnumerable<PurchaseOrderDelivery> SearchDeliveries(
            string supplierSearchTerm,
            string orderNumberSearchTerm,
            bool includeAcknowledged,
            bool? exactOrderNumber = false)
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
                result = exactOrderNumber.GetValueOrDefault() 
                             ? result.Where(x => x.OrderNumber.ToString().Equals(orderNumberSearchTerm)) 
                             : result.Where(x => x.OrderNumber.ToString().Contains(orderNumberSearchTerm));
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

            if (!this.purchaseLedgerMaster.GetRecord().OkToRaiseOrder.Equals("Y"))
            {
                throw new UnauthorisedActionException("Orders are currently restricted.");
            }

            var entity = this.repository.FindById(key);
            var miniOrder = this.miniOrderRepository.FindById(key.OrderNumber);
            var miniOrderDelivery = this.miniOrderDeliveryRepository.FindBy(
                x => x.OrderNumber == key.OrderNumber && x.DeliverySequence == key.DeliverySequence);

            if (from.DateAdvised != to.DateAdvised)
            {
                entity.DateAdvised = to.DateAdvised;
                miniOrder.AdvisedDeliveryDate = to.DateAdvised;
                miniOrderDelivery.AdvisedDate = to.DateAdvised;
            }

            if (from.RescheduleReason != to.RescheduleReason)
            {
                entity.RescheduleReason = to.RescheduleReason;
            }

            if (from.SupplierConfirmationComment != to.SupplierConfirmationComment)
            {
                entity.SupplierConfirmationComment = to.SupplierConfirmationComment;
                miniOrder.AcknowledgeComment = to.SupplierConfirmationComment;
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

            if (!this.purchaseLedgerMaster.GetRecord().OkToRaiseOrder.Equals("Y"))
            {
                throw new UnauthorisedActionException("Orders are currently restricted.");
            }

            var successCount = 0;

            var errors = new List<Error>();

            var purchaseOrderDeliveryUpdates = changes as PurchaseOrderDeliveryUpdate[] ?? changes.ToArray();
            foreach (var change in purchaseOrderDeliveryUpdates)
            {
                var entity = this.repository.FindById(change.Key);
                var miniOrder = this.miniOrderRepository.FindById(change.Key.OrderNumber);
                var miniOrderDelivery = this.miniOrderDeliveryRepository.FindBy(
                    x => x.OrderNumber == change.Key.OrderNumber && x.DeliverySequence == change.Key.DeliverySequence);

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
                else if (!this.rescheduleReasonRepository.FindAll().Select(r => r.Reason).Contains(change.NewReason))
                {
                    errors.Add(new Error(
                        $"{change.Key.OrderNumber} / {change.Key.OrderLine} / {change.Key.DeliverySequence}", 
                        $"{change.NewReason} is not a valid reason"));
                }
                else
                {
                    entity.DateAdvised = change.NewDateAdvised;
                    entity.RescheduleReason = change.NewReason;
                    miniOrder.AdvisedDeliveryDate = change.NewDateAdvised;
                    miniOrderDelivery.AdvisedDate = change.NewDateAdvised;
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

        public IEnumerable<PurchaseOrderDelivery> UpdateDeliveriesForOrderLine(
            int orderNumber,
            int orderLine,
            IEnumerable<PurchaseOrderDelivery> updated,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to split deliveries");
            }

            var order = this.purchaseOrderRepository
                .FindById(orderNumber);
            var detail = order?.Details.SingleOrDefault(x => x.Line == orderLine);

            if (detail == null)
            {
                throw new PurchaseOrderDeliveryException($"order line not found: {orderNumber} / {orderLine}.");
            }

            if (order.OrderMethod.Name.Equals("CALL OFF"))
            {
                throw new PurchaseOrderDeliveryException(
                    "You cannot raise a split delivery for a CALL OFF. It is raised automatically on delivery.");
            }

            var updateDeliveriesForOrderLine = updated.ToList();

            if (detail.OurQty.GetValueOrDefault() != updateDeliveriesForOrderLine
                    .Sum(x => x.OurDeliveryQty.GetValueOrDefault()))
            {
                throw new PurchaseOrderDeliveryException(
                    "You must match the order qty when splitting deliveries.");
            }

            detail.PurchaseDeliveries = updateDeliveriesForOrderLine;

            return detail.PurchaseDeliveries;
        }
    }
}

