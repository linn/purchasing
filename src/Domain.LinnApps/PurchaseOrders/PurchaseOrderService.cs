namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;

    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<CancelledOrderDetail, int> cancelledDetailRepository;

        private readonly IDatabaseService databaseService;

        private readonly IRepository<PurchaseOrder, int> orderRepository;

        private readonly IPurchaseLedgerPack purchaseLedgerPack;

        public PurchaseOrderService(
            IAuthorisationService authService,
            IRepository<CancelledOrderDetail, int> cancelledDetailRepository,
            IRepository<PurchaseOrder, int> orderRepository,
            IPurchaseLedgerPack purchaseLedgerPack,
            IDatabaseService databaseService)
        {
            this.authService = authService;
            this.cancelledDetailRepository = cancelledDetailRepository;
            this.orderRepository = orderRepository;
            this.purchaseLedgerPack = purchaseLedgerPack;
            this.databaseService = databaseService;
        }

        public void AllowOverbook(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to allow overbooks on a purchase order");
            }

            current.Overbook = updated.Overbook;
            current.OverbookQty = updated.OverbookQty;
        }

        public PurchaseOrder CancelOrder(PurchaseOrder order, int currentUserId, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to cancel purchase orders");
            }

            order.Cancelled = "Y";

            // for each line on order
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
                                              ReasonCancelled = "todo", // detail.ReasonCancelled,
                                              ValueCancelled = detail.BaseDetailTotal
                                          };

                // todo check that baseDetailTotal is   round(nvl(v_qty_outstanding, 0) * :new.base_our_price, 2)
                detail.CancelledDetails.Add(cancelledDetail);

                // check above works and this can then go:
                // this.cancelledDetailRepository.Add(cancelledDetail);
            }

            return order;
        }

        public PurchaseOrder Create(PurchaseOrder newOrder, IEnumerable<string> privileges)
        {
            //todo move to facade
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create purchase orders");
            }

            // create order
            this.orderRepository.Add(newOrder);

            // will this add the details etc?

            // create detail(s)

            // create deliveries maybe only if split deliveries is 0? Are split delivieres pl deliveries or something else?

            // create pl_order_postings
            // select plorp_seq.nextval into v_plorp from dual;
            return newOrder;
        }

        public PurchaseOrder Update(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update purchase orders");
            }

            this.UpdateTopLevelProperties(current, updated);
            this.UpdateDetails(current.Details, updated.Details);

            return current;
        }

        private void UpdateDetails(IList<PurchaseOrderDetail> currentDetails, IList<PurchaseOrderDetail> updatedDetails)
        {
            foreach (var detail in updatedDetails)
            {
                if (currentDetails.Any(x => x.Line == detail.Line))
                {
                    var current = currentDetails.First(x => x.Line == detail.Line);
                    current.PartNumber = detail.PartNumber;

                    // update rest
                    // + update deliveries etc
                }
                else
                {
                    currentDetails.Add(detail);
                }
            }
        }

        private void UpdateTopLevelProperties(PurchaseOrder current, PurchaseOrder updated)
        {
            current.AuthorisedById = updated.AuthorisedById;

            // current.
        }
    }
}
