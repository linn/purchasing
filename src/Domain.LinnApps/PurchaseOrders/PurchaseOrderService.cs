namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
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

        public void UpdatePurchaseOrder(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to Overbook records");
            }

            current.Overbook = updated.Overbook;
            current.OverbookQty = updated.OverbookQty;
        }
    }
}
