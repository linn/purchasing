namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IAuthorisationService authService;

        private readonly IDatabaseService databaseService;

        private readonly IPurchaseLedgerPack purchaseLedgerPack;

        public PurchaseOrderService(
            IAuthorisationService authService,
            IPurchaseLedgerPack purchaseLedgerPack,
            IDatabaseService databaseService)
        {
            this.authService = authService;
            this.purchaseLedgerPack = purchaseLedgerPack;
            this.databaseService = databaseService;
        }

        public void AllowOverbook(
            PurchaseOrder current,
            string allowOverBook,
            decimal? overbookQty,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to allow overbooks on a purchase order");
            }

            current.Overbook = allowOverBook;
            current.OverbookQty = overbookQty;
        }

        public void CreateOrder(PurchaseOrder order, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create purchase orders");
            }
            // add id to pl_order_postings using next val plorp_seq

            //select nomacc_id
            //from nominal_accounts where
            //    nominal = p_nom and department = p_dept;
        }

        public PurchaseOrder CancelOrder(PurchaseOrder order, int currentUserId, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to cancel purchase orders");
            }

            order.Cancelled = "Y";

            var currentLedgerPeriod = this.purchaseLedgerPack.GetLedgerPeriod();

            foreach (var detail in order.Details)
            {
                var cancelledDetail = new CancelledOrderDetail
                                          {
                                              Id = this.databaseService.GetNextVal("PLOC_SEQ"),
                                              OrderNumber = detail.OrderNumber,
                                              LineNumber = detail.Line,
                                              DateCancelled = DateTime.Today,
                                              PeriodCancelled = currentLedgerPeriod,
                                              CancelledById = currentUserId,
                                              ReasonCancelled = detail.Cancelled,
                                              ValueCancelled = detail.BaseDetailTotal
                                              //// todo check for valuecancelled that:
                                              // baseDetailTotal == round(nvl(v_qty_outstanding, 0) * :new.base_our_price, 2)
                                          };
                detail.Cancelled = "Y";
                detail.CancelledDetails.Add(cancelledDetail);
            }

            return order;
        }

        public PurchaseOrder UpdateOrder(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update purchase orders");
            }

            this.UpdateOrderProperties(current, updated);
            this.UpdateDetails(current.Details, updated.Details);
            //Update pl_order_postings? Or just on create? todo investigate

            return current;
        }

        private void UpdateDetails(IList<PurchaseOrderDetail> currentDetails, IList<PurchaseOrderDetail> updatedDetails)
        {
            foreach (var detail in updatedDetails)
            {
                var currentDetail = currentDetails.FirstOrDefault(x => x.Line == detail.Line);
                if (currentDetail != null)
                {
                    this.UpdateDetailProperties(currentDetail, detail);

                    foreach (var delivery in detail.PurchaseDeliveries)
                    {
                        var currentDelivery = currentDetail.PurchaseDeliveries.FirstOrDefault(x => x.DeliverySeq == delivery.DeliverySeq);
                        if (currentDelivery != null)
                        {
                            this.UpdateDeliveryProperties(currentDelivery, delivery);
                        }
                        else
                        {
                            ////todo check delivery.DeliverySeq of new delivery object is all good once front end done, same with details
                            currentDetail.PurchaseDeliveries.Add(delivery);
                        }
                    }
                }
                else
                {
                    currentDetails.Add(detail);
                }
            }
        }

        private void UpdateOrderProperties(PurchaseOrder current, PurchaseOrder updated)
        {
            current.Cancelled = updated.Cancelled;
            current.DocumentTypeName = updated.DocumentTypeName;
            current.OrderDate = updated.OrderDate;
            current.SupplierId = updated.SupplierId;
            current.CurrencyCode = updated.CurrencyCode;
            current.OrderContactName = updated.OrderContactName;
            current.OrderMethodName = updated.OrderMethodName;
            current.ExchangeRate = updated.ExchangeRate;
            current.IssuePartsToSupplier = updated.IssuePartsToSupplier;
            current.DeliveryAddressId = updated.DeliveryAddressId;
            current.RequestedById = updated.RequestedById;
            current.EnteredById = updated.EnteredById;
            current.QuotationRef = updated.QuotationRef;
            current.AuthorisedById = updated.AuthorisedById;
            current.SentByMethod = updated.SentByMethod;
            current.FilCancelled = updated.FilCancelled;
            current.Remarks = updated.Remarks;
            current.DateFilCancelled = updated.DateFilCancelled;
            current.PeriodFilCancelled = updated.PeriodFilCancelled;
        }

        private void UpdateDeliveryProperties(PurchaseOrderDelivery current, PurchaseOrderDelivery updated)
        {
            current.Cancelled = updated.Cancelled;
            current.DateAdvised = updated.DateAdvised;
            current.DateRequested = updated.DateRequested;
            current.NetTotalCurrency = updated.NetTotalCurrency;
            current.BaseNetTotal = updated.BaseNetTotal;
            current.OrderDeliveryQty = updated.OrderDeliveryQty;
            current.OurDeliveryQty = updated.OurDeliveryQty;
            current.QtyNetReceived = updated.QtyNetReceived;
            current.QuantityOutstanding = updated.QuantityOutstanding;
            current.CallOffDate = updated.CallOffDate;
            current.BaseOurUnitPrice = updated.BaseOurUnitPrice;
            current.SupplierConfirmationComment = updated.SupplierConfirmationComment;
            current.OurUnitPriceCurrency = updated.OurUnitPriceCurrency;
            current.OrderUnitPriceCurrency = updated.OrderUnitPriceCurrency;
            current.BaseOrderUnitPrice = updated.BaseOrderUnitPrice;
            current.VatTotalCurrency = updated.VatTotalCurrency;
            current.BaseVatTotal = updated.BaseVatTotal;
            current.DeliveryTotalCurrency = updated.DeliveryTotalCurrency;
            current.BaseDeliveryTotal = updated.BaseDeliveryTotal;
        }

        private void UpdateDetailProperties(PurchaseOrderDetail current, PurchaseOrderDetail updated)
        {
            current.PartNumber = updated.PartNumber;
            current.Cancelled = updated.Cancelled; 
            current.BaseNetTotal = updated.BaseNetTotal;
            current.NetTotalCurrency = updated.NetTotalCurrency;
            current.OurQty = updated.OurQty;
            current.PartNumber = updated.PartNumber;
            current.RohsCompliant = updated.RohsCompliant;
            current.SuppliersDesignation = updated.SuppliersDesignation;
            current.StockPoolCode = updated.StockPoolCode;
            current.OriginalOrderNumber = updated.OriginalOrderNumber;
            current.OriginalOrderLine = updated.OriginalOrderLine;
            current.OurUnitOfMeasure = updated.OurUnitOfMeasure;
            current.OrderUnitOfMeasure = updated.OrderUnitOfMeasure;
            current.Duty = updated.Duty;
            current.OurUnitPriceCurrency = updated.OurUnitPriceCurrency;
            current.OrderUnitPriceCurrency = updated.OrderUnitPriceCurrency;
            current.BaseOurUnitPrice = updated.BaseOurUnitPrice;
            current.BaseOrderUnitPrice = updated.BaseOrderUnitPrice;
            current.VatTotalCurrency = updated.VatTotalCurrency;
            current.BaseVatTotal = updated.BaseVatTotal;
            current.DetailTotalCurrency = updated.DetailTotalCurrency;
            current.BaseDetailTotal = updated.BaseDetailTotal;
            current.DeliveryInstructions = updated.DeliveryInstructions;
            current.DeliveryConfirmedBy = updated.DeliveryConfirmedBy;
            current.DeliveryConfirmedById = updated.DeliveryConfirmedById;
            current.InternalComments = updated.InternalComments;
        }
    }
}
