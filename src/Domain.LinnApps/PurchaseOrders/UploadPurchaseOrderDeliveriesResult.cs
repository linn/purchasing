namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public class UploadPurchaseOrderDeliveriesResult : BatchUpdateProcessResult
    {
        public IEnumerable<PurchaseOrderDelivery> Updated { get; set; }
    }
}
