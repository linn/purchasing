namespace Linn.Purchasing.Resources.RequestResources
{
    using System.Collections.Generic;

    public class PatchPurchaseOrderDeliveryResource
    {
        public IEnumerable<PurchaseOrderDeliveryResource> From { get; set; }

        public IEnumerable<PurchaseOrderDeliveryResource> To { get; set; }
    }
}
