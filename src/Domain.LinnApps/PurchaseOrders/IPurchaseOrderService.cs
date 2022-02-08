namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public interface IPurchaseOrderService
    {
        public void AllowOverbook(PurchaseOrder current, PurchaseOrder updated, IEnumerable<string> privileges);
    }
}
