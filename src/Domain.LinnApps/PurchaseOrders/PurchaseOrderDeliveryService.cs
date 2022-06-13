﻿namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
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
        private readonly IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository;

        private readonly IAuthorisationService authService;

        private readonly IRepository<RescheduleReason, string> rescheduleReasonRepository;

        private readonly ISingleRecordRepository<PurchaseLedgerMaster> purchaseLedgerMaster;

        private readonly IRepository<MiniOrder, int> miniOrderRepository;

        private readonly IRepository<MiniOrderDelivery, MiniOrderDeliveryKey> miniOrderDeliveryRepository;

        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        private readonly IPurchaseOrdersPack purchaseOrdersPack;

        private readonly IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> deliveryRepository;

        public PurchaseOrderDeliveryService(
            IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> repository,
            IAuthorisationService authService,
            IRepository<RescheduleReason, string> rescheduleReasonRepository,
            ISingleRecordRepository<PurchaseLedgerMaster> purchaseLedgerMaster,
            IRepository<MiniOrder, int> miniOrderRepository,
            IRepository<MiniOrderDelivery, MiniOrderDeliveryKey> miniOrderDeliveryRepository,
            IRepository<PurchaseOrder, int> purchaseOrderRepository,
            IPurchaseOrdersPack purchaseOrdersPack,
            IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> deliveryRepository)
        {
            this.repository = repository;
            this.authService = authService;
            this.rescheduleReasonRepository = rescheduleReasonRepository;
            this.purchaseLedgerMaster = purchaseLedgerMaster;
            this.miniOrderRepository = miniOrderRepository;
            this.miniOrderDeliveryRepository = miniOrderDeliveryRepository;
            this.purchaseOrderRepository = purchaseOrderRepository;
            this.purchaseOrdersPack = purchaseOrdersPack;
            this.deliveryRepository = deliveryRepository;
        }

        public IEnumerable<PurchaseOrderDelivery> SearchDeliveries(
            string supplierSearchTerm,
            string orderNumberSearchTerm,
            bool includeAcknowledged,
            bool? exactOrderNumber = false,
            int? orderLine = null)
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
                if (orderLine.HasValue)
                {
                    result = result.Where(x => x.OrderLine == orderLine);
                }
            }

            if (!includeAcknowledged)
            {
                result = result.Where(x => !x.DateAdvised.HasValue);
            }

            return result.OrderBy(x => x.OrderNumber).ThenBy(x => x.OrderLine).ThenBy(x => x.DeliverySeq);
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
                    to.SupplierConfirmationComment,
                    null);
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

            this.CheckOkToRaiseOrders();

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
                    this.UpdateMiniOrder(
                        change.Key.OrderNumber, 
                        change.NewDateAdvised, 
                        null, 
                        null, 
                        null, 
                        null);
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
            var detail = order?.Details?.SingleOrDefault(x => x.Line == orderLine);

            if (detail == null)
            {
                throw new PurchaseOrderDeliveryException($"order line not found: {orderNumber} / {orderLine}.");
            }

            if (order.OrderMethod?.Name == "CALL OFF")
            {
                throw new PurchaseOrderDeliveryException(
                    "You cannot raise a split delivery for a CALL OFF. It is raised automatically on delivery.");
            }

            if (order.Cancelled == "Y")
            {
                throw new PurchaseOrderDeliveryException("Cannot split deliveries - Order is cancelled.");
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
                    "You must match the order qty when splitting deliveries.");
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
                            existing = this.deliveryRepository.FindBy(
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
            // and write the new deliveries list to the mini order to keep it in sync
            this.UpdateMiniOrder(
                orderNumber, 
                null,
                updatedDeliveriesForOrderLine.MinBy(x => x.DateRequested)?.DateRequested, 
                updatedDeliveriesForOrderLine.Count, 
                null,
                updatedDeliveriesForOrderLine);

            return detail.PurchaseDeliveries;
        }

        private void CheckOkToRaiseOrders()
        {
            if (!this.purchaseLedgerMaster.GetRecord().OkToRaiseOrder.Equals("Y"))
            {
                throw new UnauthorisedActionException("Orders are currently restricted.");
            }
        }

        // syncs any delivery changes back to the mini_order
        // keeping this 'temporary' code in a dedicated method so it's clear and easy to remove
        private void UpdateMiniOrder(
            int orderNumber, 
            DateTime? advisedDeliveryDate, 
            DateTime? requestedDate, 
            int? numberOfSplitDeliveries,
            string supplierConfirmationComment,
            IEnumerable<PurchaseOrderDelivery> updatedDeliveries)
        {
            var miniOrder = this.miniOrderRepository.FindById(orderNumber);
            var miniOrderDelivery = this.miniOrderDeliveryRepository.FindBy(
                x => x.OrderNumber == orderNumber && x.DeliverySequence == 1);

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
                miniOrderDelivery.AdvisedDate = advisedDeliveryDate;
                miniOrder.AdvisedDeliveryDate = advisedDeliveryDate;
            }

            if (supplierConfirmationComment != null)
            {
                miniOrder.AcknowledgeComment = supplierConfirmationComment;
            }

            if (updatedDeliveries != null)
            {
                miniOrder.Deliveries = updatedDeliveries
                    .Select(del =>
                        {
                            var existing =
                                miniOrder.Deliveries?.FirstOrDefault(d => d.DeliverySequence == del.DeliverySeq);

                            if (existing != null)
                            {
                                existing = this.miniOrderDeliveryRepository.FindBy(
                                    d => d.OrderNumber == orderNumber && d.DeliverySequence == del.DeliverySeq);
                                existing.AdvisedDate = del.DateAdvised;
                                existing.RequestedDate = del.DateRequested;
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
        }
    }
}

