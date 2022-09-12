namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    public class PurchaseOrderDeliveryService : IPurchaseOrderDeliveryService
    {
        private readonly IPurchaseOrderDeliveryRepository repository;

        private readonly IAuthorisationService authService;

        private readonly ISingleRecordRepository<PurchaseLedgerMaster> purchaseLedgerMaster;

        private readonly IRepository<MiniOrder, int> miniOrderRepository;

        private readonly IRepository<MiniOrderDelivery, MiniOrderDeliveryKey> miniOrderDeliveryRepository;

        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        private readonly IPurchaseOrdersPack purchaseOrdersPack;

        public PurchaseOrderDeliveryService(
            IPurchaseOrderDeliveryRepository repository,
            IAuthorisationService authService,
            ISingleRecordRepository<PurchaseLedgerMaster> purchaseLedgerMaster,
            IRepository<MiniOrder, int> miniOrderRepository,
            IRepository<MiniOrderDelivery, MiniOrderDeliveryKey> miniOrderDeliveryRepository,
            IRepository<PurchaseOrder, int> purchaseOrderRepository,
            IPurchaseOrdersPack purchaseOrdersPack)
        {
            this.repository = repository;
            this.authService = authService;
            this.purchaseLedgerMaster = purchaseLedgerMaster;
            this.miniOrderRepository = miniOrderRepository;
            this.miniOrderDeliveryRepository = miniOrderDeliveryRepository;
            this.purchaseOrderRepository = purchaseOrderRepository;
            this.purchaseOrdersPack = purchaseOrdersPack;
        }

        public IEnumerable<PurchaseOrderDelivery> SearchDeliveries(
            string supplierSearchTerm,
            string orderNumberSearchTerm,
            bool includeAcknowledged,
            int? orderLine = null)
        {
            var result = this.repository.FindAll();

            if (!string.IsNullOrEmpty(orderNumberSearchTerm))
            {
                if (orderNumberSearchTerm.Contains("*"))
                {
                    result = this.repository.SearchByOrderWithWildcard(orderNumberSearchTerm
                        .Replace("*", "%")).AsQueryable();
                }
                else
                {
                    result = result.Where(x => x.OrderNumber.ToString().Equals(orderNumberSearchTerm));
                }

                if (orderLine.HasValue)
                {
                    result = result.Where(x => x.OrderLine == orderLine);
                }
            }

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

            if (!includeAcknowledged)
            {
                result = result.Where(x => !x.DateAdvised.HasValue);
            }

            return result
                .Where(x => x.Cancelled != "Y")
                .OrderBy(x => x.OrderNumber).ThenBy(x => x.OrderLine).ThenBy(x => x.DeliverySeq);
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

            if (from.DateAdvised != to.DateAdvised)
            {
                entity.DateAdvised = to.DateAdvised;
                this.UpdateMiniOrder(
                    key.OrderNumber,
                    to.DateAdvised,
                    null,
                    null,
                    null);
            }

            if (from.RescheduleReason != to.RescheduleReason)
            {
                entity.RescheduleReason = to.RescheduleReason;
            }

            if (from.SupplierConfirmationComment != to.SupplierConfirmationComment)
            {
                entity.SupplierConfirmationComment = to.SupplierConfirmationComment;
                this.UpdateMiniOrder(
                    key.OrderNumber,
                    null,
                    null,
                    null,
                    to.SupplierConfirmationComment);
            }

            if (from.AvailableAtSupplier != to.AvailableAtSupplier)
            {
                entity.AvailableAtSupplier = to.AvailableAtSupplier;
            }

            return entity;
        }

        public UploadPurchaseOrderDeliveriesResult UploadDeliveries(
            IEnumerable<PurchaseOrderDeliveryUpdate> changes,
            IEnumerable<string> privileges)
        {
            var updated = new List<PurchaseOrderDelivery>();

            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to acknowledge orders.");
            }

            this.CheckOkToRaiseOrders();

            var successCount = 0;

            var errors = new List<Error>();

            var purchaseOrderDeliveryUpdates = changes as PurchaseOrderDeliveryUpdate[] ?? changes.ToArray();

            var orderLineGroups = purchaseOrderDeliveryUpdates
                .GroupBy(x => new { x.Key.OrderNumber, x.Key.OrderLine })
                .Select(group => new
                                     {
                                         group.Key.OrderNumber,
                                         group.Key.OrderLine,
                                         DeliveryUpdates = group.OrderBy(x => x.NewDateAdvised)
                                     });

            foreach (var group in orderLineGroups)
            {
                var existingDeliveries = this.repository.FilterBy(
                    x => x.OrderNumber == group.OrderNumber && x.OrderLine == group.OrderLine);

                if (existingDeliveries.Any(x => x.QtyNetReceived.GetValueOrDefault() > 0))
                {
                    errors.Add(
                        new Error(
                            $"Order: {group.OrderNumber}",
                            "Order has been partially received. Cannot update deliveries automatically."));
                    continue;
                }

                var detail = this.purchaseOrderRepository.FindById(group.OrderNumber).Details
                    .Single(d => d.Line == group.OrderLine);

                var qtyOutstanding = detail.OrderQty - existingDeliveries.Sum(x => x.QtyNetReceived);

                var totalQty = group.DeliveryUpdates.Sum(x => x.Qty);

                var isQuantityMismatch = totalQty
                                         != qtyOutstanding;

                if (isQuantityMismatch)
                {
                    var msg = $"Total Qty of lines uploaded ({totalQty}) does not match order qty outstanding ({qtyOutstanding})";
                    errors.Add(
                        new Error(
                            $"Order: {group.OrderNumber}",
                            msg));
                    continue;
                }

                var existingDelivery = existingDeliveries.First();

                var isPricesMismatch = group.DeliveryUpdates.Any(
                    u => Math.Round(u.UnitPrice, 4) != Math.Round(
                             detail.OrderUnitPriceCurrency.GetValueOrDefault(),
                             4));

                if (isPricesMismatch)
                {
                    var msg = $"Unit Price on lines uploaded ({group.DeliveryUpdates.First().UnitPrice}) "
                              + $"for the specified order does not match unit price on our system " 
                              + $"({detail.OrderUnitPriceCurrency})";
                    errors.Add(
                        new Error(
                            $"Order: {group.OrderNumber}",
                            msg));
                    continue;
                }

                var updates = group.DeliveryUpdates.OrderBy(u => u.NewDateAdvised).ToList();
                var index = 1;
                var updatedDeliveries = new List<PurchaseOrderDelivery>();
                
                foreach (var update in updates)
                {
                    var vatAmount = Math.Round(
                        this.purchaseOrdersPack.GetVatAmountSupplier(
                            detail.OrderUnitPriceCurrency.GetValueOrDefault() * update.Qty,
                            existingDelivery.PurchaseOrderDetail.PurchaseOrder.SupplierId),
                        2);

                    var baseVatAmount = Math.Round(
                        this.purchaseOrdersPack.GetVatAmountSupplier(
                            detail.BaseOurUnitPrice.GetValueOrDefault() * update.Qty,
                            existingDelivery.PurchaseOrderDetail.PurchaseOrder.SupplierId),
                        2);
                    updatedDeliveries.Add(new PurchaseOrderDelivery
                                              {
                                                  OrderNumber = group.OrderNumber,
                                                  OrderLine = group.OrderLine,
                                                  DeliverySeq = index,
                                                  OurDeliveryQty = update.Qty,
                                                  QtyNetReceived = 0,
                                                  QuantityOutstanding = update.Qty,
                                                  DateAdvised = update.NewDateAdvised,
                                                  AvailableAtSupplier = update.AvailableAtSupplier,
                                                  RescheduleReason = update.NewReason,
                                                  DateRequested = update.DateRequested ?? existingDelivery.DateRequested,
                                                  SupplierConfirmationComment = update.Comment,
                                                  OurUnitPriceCurrency = detail.OurUnitPriceCurrency,
                                                  OrderUnitPriceCurrency = detail.OrderUnitPriceCurrency,
                                                  BaseOurUnitPrice = detail.BaseOurUnitPrice,
                                                  BaseDeliveryTotal = Math.Round(
                                                      (update.Qty * detail.BaseOurUnitPrice.GetValueOrDefault())
                                                    + baseVatAmount, 
                                                      2),
                                                  BaseNetTotal = Math.Round(
                                                      update.Qty * detail.BaseOurUnitPrice.GetValueOrDefault(),
                                                      2),
                                                  OrderDeliveryQty = update.Qty / detail.OrderConversionFactor,
                                                  BaseOrderUnitPrice = existingDelivery.BaseOrderUnitPrice,
                                                  VatTotalCurrency = vatAmount,
                                                  BaseVatTotal = baseVatAmount,
                                                  CallOffDate = existingDelivery.CallOffDate,
                                                  CallOffRef = existingDelivery.CallOffRef,
                                                  Cancelled = "N",
                                                  DeliveryTotalCurrency = Math.Round(
                                                                              detail.OrderUnitPriceCurrency.GetValueOrDefault() * update.Qty,
                                                                              2) + vatAmount,
                                                  FilCancelled = "N",
                                                  QtyPassedForPayment = 0,
                                                  NetTotalCurrency = update.Qty * update.UnitPrice
                                              });
                        index++;
                }

                detail.PurchaseDeliveries = updatedDeliveries;

                    this.UpdateMiniOrder(
                        existingDelivery.OrderNumber,
                        updates.Last().NewDateAdvised,
                        null,
                        null,
                        updates.Last().Comment);
                updated?.AddRange(detail?.PurchaseDeliveries);
                successCount++;
            }

            if (errors.Any())
            {
                return new UploadPurchaseOrderDeliveriesResult
                           {
                               Success = false,
                               Message =
                                   $"{successCount} orders updated successfully. The following errors occurred: ",
                               Errors = errors,
                               Updated = updated
                           };
            }

            return new UploadPurchaseOrderDeliveriesResult
                       {
                           Success = true, 
                           Message = $"{successCount} orders updated successfully.",
                           Updated = updated
                       };
        }

        public BatchUpdateProcessResult UpdateDeliveries(
           IEnumerable<PurchaseOrderDeliveryUpdate> changes,
           IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to acknowledge orders.");
            }

            this.CheckOkToRaiseOrders();

            var successCount = 0;

            var purchaseOrderDeliveryUpdates = changes as PurchaseOrderDeliveryUpdate[] ?? changes.ToArray();

            var orderLineGroups = purchaseOrderDeliveryUpdates
                .GroupBy(x => new { x.Key.OrderNumber, x.Key.OrderLine })
                .Select(group => new
                {
                    group.Key.OrderNumber,
                    group.Key.OrderLine,
                    DeliveryUpdates = group
                });

            foreach (var group in orderLineGroups)
            {
                foreach (var u in group.DeliveryUpdates)
                {
                    var deliveryToUpdate = this.repository.FindBy(x =>
                        x.OrderNumber == u.Key.OrderNumber
                        && x.OrderLine == u.Key.OrderLine
                        && x.DeliverySeq == u.Key.DeliverySequence);

                    if (!string.IsNullOrEmpty(u.Comment))
                    {
                        deliveryToUpdate.SupplierConfirmationComment = u.Comment;
                    }

                    if (!string.IsNullOrEmpty(u.AvailableAtSupplier))
                    {
                        deliveryToUpdate.AvailableAtSupplier = u.AvailableAtSupplier;
                    }

                    deliveryToUpdate.DateAdvised = u.NewDateAdvised;
                    deliveryToUpdate.RescheduleReason = u.NewReason;
                    this.UpdateMiniOrder(
                        u.Key.OrderNumber,
                        u.NewDateAdvised,
                        null,
                        null,
                        u.Comment);
                    successCount++;
                }
            }

            return new BatchUpdateProcessResult
            {
                Success = true,
                Message = $"{successCount} records updated successfully."
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
                throw new UnauthorisedActionException("You are not authorised to update deliveries");
            }

            var order = this.purchaseOrderRepository
                .FindById(orderNumber);
            var detail = order?.Details?.SingleOrDefault(x => x.Line == orderLine);

            if (detail == null)
            {
                throw new PurchaseOrderDeliveryException($"order line not found: {orderNumber} / {orderLine}.");
            }

            if (order.OrderMethod?.Name == "CALL OFF")
            {
                throw new PurchaseOrderDeliveryException(
                    "You cannot update deliveries for a CALL OFF.");
            }

            if (order.Cancelled == "Y")
            {
                throw new PurchaseOrderDeliveryException("Cannot update deliveries - Order is cancelled.");
            }

            if (order.DocumentTypeName != "PO")
            {
                throw new PurchaseOrderDeliveryException("Cannot split deliveries - Order is not a PO.");
            }

            var updatedDeliveriesForOrderLine = updated.ToList();

            if (detail.OurQty.GetValueOrDefault() != updatedDeliveriesForOrderLine
                    .Sum(x => x.OurDeliveryQty.GetValueOrDefault()))
            {
                throw new PurchaseOrderDeliveryException(
                    "You must match the order qty when updating deliveries.");
            }

            var list = detail.PurchaseDeliveries.ToArray();

            var newDeliveries = updatedDeliveriesForOrderLine.Select(
                del =>
                    {
                        var existing = list.FirstOrDefault(x => x.DeliverySeq == del.DeliverySeq);
                        var vatAmount = Math.Round(
                            this.purchaseOrdersPack.GetVatAmountSupplier(
                                detail.OrderUnitPriceCurrency.GetValueOrDefault() * del.OurDeliveryQty.GetValueOrDefault(),
                                order.SupplierId),
                            2);

                        var baseVatAmount = Math.Round(
                            this.purchaseOrdersPack.GetVatAmountSupplier(
                                detail.BaseOurUnitPrice.GetValueOrDefault() * del.OurDeliveryQty.GetValueOrDefault(),
                                order.SupplierId),
                            2);
                        var reason = del.DateAdvised.HasValue ? "ADVISED" : "REQUESTED";

                        // update the existing record if it exists
                        if (existing != null)
                        {
                            var seq = existing.DeliverySeq;
                            existing = this.repository.FindBy(
                                d => d.OrderNumber == orderNumber && d.OrderLine == orderLine
                                                                  && d.DeliverySeq == seq);
                            existing.OurDeliveryQty = del.OurDeliveryQty;
                            existing.OrderDeliveryQty = del.OurDeliveryQty / detail.OrderConversionFactor;
                            existing.OurUnitPriceCurrency = detail.OurUnitPriceCurrency;
                            existing.DateRequested = del.DateRequested;
                            existing.DateAdvised = del.DateAdvised;
                            existing.CallOffDate = DateTime.Now;
                            existing.NetTotalCurrency = Math.Round(
                                del.OurDeliveryQty.GetValueOrDefault()
                                * detail.OurUnitPriceCurrency.GetValueOrDefault(),
                                2);
                            existing.VatTotalCurrency = vatAmount;
                            existing.DeliveryTotalCurrency = Math.Round(
                                                                 detail.OrderUnitPriceCurrency.GetValueOrDefault()
                                                                 * del.OurDeliveryQty.GetValueOrDefault(),
                                                                 2) + vatAmount;
                            existing.BaseOurUnitPrice = detail.BaseOurUnitPrice;
                            existing.BaseOrderUnitPrice = detail.BaseOrderUnitPrice;
                            existing.BaseNetTotal = Math.Round(
                                del.OurDeliveryQty.GetValueOrDefault() * detail.BaseOurUnitPrice.GetValueOrDefault(),
                                2);
                            existing.BaseVatTotal = baseVatAmount;
                            existing.BaseDeliveryTotal = Math.Round(
                                (del.OurDeliveryQty.GetValueOrDefault() * detail.BaseOurUnitPrice.GetValueOrDefault())
                                + baseVatAmount,
                                2);
                            existing.QuantityOutstanding =
                                del.OurDeliveryQty - existing.OurDeliveryQty + existing.QuantityOutstanding;
                            existing.RescheduleReason = reason;
                            existing.AvailableAtSupplier = del.AvailableAtSupplier;
                            return existing;
                        }

                        // or create a new record
                        return new PurchaseOrderDelivery
                                         {
                                             DeliverySeq = del.DeliverySeq,
                                             OurDeliveryQty = del.OurDeliveryQty,
                                             OrderDeliveryQty = del.OurDeliveryQty / detail.OrderConversionFactor,
                                             OurUnitPriceCurrency = detail.OurUnitPriceCurrency,
                                             OrderUnitPriceCurrency = detail.OrderUnitPriceCurrency,
                                             DateRequested = del.DateRequested,
                                             DateAdvised = del.DateAdvised,
                                             CallOffDate = DateTime.Now,
                                             Cancelled = "N",
                                             CallOffRef = null,
                                             OrderNumber = orderNumber,
                                             OrderLine = orderLine,
                                             FilCancelled = "N",
                                             NetTotalCurrency = Math.Round(
                                                 del.OurDeliveryQty.GetValueOrDefault()
                                                 * detail.OurUnitPriceCurrency.GetValueOrDefault(),
                                                 2),
                                             VatTotalCurrency = vatAmount,
                                             DeliveryTotalCurrency = Math.Round(
                                                 detail.OrderUnitPriceCurrency.GetValueOrDefault() * del.OurDeliveryQty.GetValueOrDefault(), 2)
                                             + vatAmount,
                                             SupplierConfirmationComment = null,
                                             BaseOurUnitPrice = del.BaseOurUnitPrice,
                                             BaseOrderUnitPrice = del.BaseOrderUnitPrice,
                                             BaseNetTotal = Math.Round(
                                                 del.OurDeliveryQty.GetValueOrDefault()
                                                 * detail.BaseOurUnitPrice.GetValueOrDefault(),
                                                 2),
                                             BaseVatTotal = baseVatAmount,
                                             BaseDeliveryTotal = Math.Round(
                                                     (del.OurDeliveryQty.GetValueOrDefault()
                                                      * detail.BaseOurUnitPrice.GetValueOrDefault())
                                                     + baseVatAmount,
                                                     2),
                                             QuantityOutstanding = del.OurDeliveryQty,
                                             QtyNetReceived = 0,
                                             QtyPassedForPayment = 0,
                                             RescheduleReason = reason,
                                             AvailableAtSupplier = del.AvailableAtSupplier
                                         };
                    });

            detail.PurchaseDeliveries = newDeliveries.ToList();

            // set mini order date requested to be first date requested of newly split deliveries
            this.UpdateMiniOrder(
                orderNumber,
                null,
                Enumerable.MinBy(updatedDeliveriesForOrderLine, x => x.DateRequested)?.DateRequested,
                updatedDeliveriesForOrderLine.Count,
                null);

            return detail.PurchaseDeliveries;
        }

        // syncs changes to the deliveries list back to the mini order
        // keeping this 'temporary' code in a dedicated method so it's clear and easy to remove
        // separate public method so that it can be called from the facade (and subsequently Commit()'ted) in the correct order
        public void UpdateMiniOrderDeliveries(IEnumerable<PurchaseOrderDelivery> updated)
        {
            var updatedPurchaseOrderDeliveries = updated.ToList();
            var miniOrder = this.miniOrderRepository.FindById(updatedPurchaseOrderDeliveries.First().OrderNumber);

            miniOrder.Deliveries = updatedPurchaseOrderDeliveries
                .Select(del =>
                    {
                        var existing =
                            miniOrder.Deliveries?.FirstOrDefault(d => d.DeliverySequence == del.DeliverySeq);

                        if (existing != null)
                        {
                            existing = this.miniOrderDeliveryRepository.FindBy(
                                d => d.OrderNumber == miniOrder.OrderNumber && d.DeliverySequence == del.DeliverySeq);
                            existing.AdvisedDate = del.DateAdvised;
                            existing.RequestedDate = del.DateRequested ?? existing.RequestedDate;
                            existing.AvailableAtSupplier = del.AvailableAtSupplier;
                            existing.OurQty = del.OurDeliveryQty;
                            return existing;
                        }

                        return new MiniOrderDelivery
                                   {
                                       AdvisedDate = del.DateAdvised,
                                       RequestedDate = del.DateRequested,
                                       DeliverySequence = del.DeliverySeq,
                                       OrderNumber = del.OrderNumber,
                                       AvailableAtSupplier = del.AvailableAtSupplier,
                                       OurQty = del.OurDeliveryQty
                                   };
                    }).ToList();
        }

        public void ReplaceMiniOrderDeliveries(IEnumerable<PurchaseOrderDelivery> updated)
        {
            var miniOrder = this.miniOrderRepository.FindById(updated.First().OrderNumber);
            miniOrder.Deliveries = updated.Select(
                del => new MiniOrderDelivery
                           {
                               AdvisedDate = del.DateAdvised,
                               RequestedDate = del.DateRequested,
                               DeliverySequence = del.DeliverySeq,
                               OrderNumber = del.OrderNumber,
                               AvailableAtSupplier = del.AvailableAtSupplier,
                               OurQty = del.OurDeliveryQty
                           }).ToList();
        }

        // syncs changes to an individual delivery back to the corresponding mini order delivery
        public void UpdateMiniOrderDelivery(
            int orderNumber, int seq, DateTime? newDateAdvised, string availableAtSupplier, decimal qty)
        {
            var miniOrder = this.miniOrderRepository.FindById(orderNumber);
            var del = miniOrder.Deliveries.FirstOrDefault(x => x.DeliverySequence == seq);
            
            if (del != null)
            {
                if (!string.IsNullOrEmpty(availableAtSupplier))
                {
                    del.AvailableAtSupplier = availableAtSupplier;
                }

                del.AdvisedDate = newDateAdvised;
            }
            else
            {
                var existing = miniOrder.Deliveries;
                this.miniOrderDeliveryRepository.Add(new MiniOrderDelivery
                                                         {
                                                             OrderNumber = orderNumber,
                                                             DeliverySequence = existing.Any() 
                                                                 ? existing.Max(d => d.DeliverySequence) + 1 : 1,
                                                             AdvisedDate = newDateAdvised,
                                                             OurQty = qty,
                                                             AvailableAtSupplier = availableAtSupplier,
                                                             RequestedDate = miniOrder.RequestedDeliveryDate
                                                         });
            }
        }

        private void CheckOkToRaiseOrders()
        {
            if (!this.purchaseLedgerMaster.GetRecord().OkToRaiseOrder.Equals("Y"))
            {
                throw new UnauthorisedActionException("Orders are currently restricted.");
            }
        }

        // syncs changes back to the mini_order
        // keeping this 'temporary' code in a dedicated method so it's clear and easy to remove
        private void UpdateMiniOrder(
            int orderNumber,
            DateTime? advisedDeliveryDate,
            DateTime? requestedDate,
            int? numberOfSplitDeliveries,
            string supplierConfirmationComment)
        {
            var miniOrder = this.miniOrderRepository.FindById(orderNumber);

            if (requestedDate.HasValue)
            {
                miniOrder.RequestedDeliveryDate = requestedDate;
            }

            if (numberOfSplitDeliveries.HasValue)
            {
                miniOrder.NumberOfSplitDeliveries = numberOfSplitDeliveries.Value;
            }

            if (advisedDeliveryDate.HasValue)
            {
                miniOrder.AdvisedDeliveryDate = advisedDeliveryDate;
            }

            if (supplierConfirmationComment != null)
            {
                miniOrder.AcknowledgeComment = supplierConfirmationComment;
            }
        }
    }
}
