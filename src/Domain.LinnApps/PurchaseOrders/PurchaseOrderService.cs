namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IAuthorisationService authService;
        private readonly IRepository<CancelledOrderDetail, int> cancelledDetailRepository;
        private readonly IRepository<PurchaseOrder, int> orderRepository;



        public PurchaseOrderService(
            IAuthorisationService authService,
            IRepository<CancelledOrderDetail, int> cancelledDetailRepository,
            IRepository<PurchaseOrder, int> orderRepository)
        {
            this.authService = authService;
            this.cancelledDetailRepository = cancelledDetailRepository;
            this.orderRepository = orderRepository;
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

        public PurchaseOrder Update(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update purchase orders");
            }

            this.UpdateTopLevelProperties(current, updated);
            //update details etc

            return current;
        }

        public PurchaseOrder Create(PurchaseOrder newOrder, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create purchase orders");
            }

            //create order

            this.orderRepository.Add(newOrder);
            //will this add the details etc?

            //create detail(s)


            //create deliveries maybe only if split deliveries is 0? Are split delivieres pl deliveries or something else?

            //create pl_order_postings
            //select plorp_seq.nextval into v_plorp from dual;



            return newOrder;
        }

        public PurchaseOrder CancelOrder(PurchaseOrder order, int currentUserId, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to cancel purchase orders");
            }

            order.Cancelled = "Y";

            //for each line on order
            //create cancelled detail 

            return order;
        }

        private void UpdateTopLevelProperties(PurchaseOrder current, PurchaseOrder updated)
        {
            current.AuthorisedById = updated.AuthorisedById;
            //current.
        }

        private void UpdateDetails(IList<PurchaseOrderDetail> currentDetails, IList<PurchaseOrderDetail> updatedDetails)
        {
            foreach (var detail in updatedDetails)
            {
                if (currentDetails.Any(x => x.Line == detail.Line))
                {
                    var current = currentDetails.First(x => x.Line == detail.Line);
                    current.PartNumber = detail.PartNumber;
                    //update rest
                }
                else
                {
                    currentDetails.Add(detail);
                }
            }
        }
    }
}
