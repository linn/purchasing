namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IAuthorisationService authService;

        public PurchaseOrderService(
            IAuthorisationService authService)
        {
            this.authService = authService;
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
    }
}
