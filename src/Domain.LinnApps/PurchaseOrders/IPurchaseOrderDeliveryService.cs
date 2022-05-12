namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public interface IPurchaseOrderDeliveryService
    {
        IEnumerable<PurchaseOrderDelivery> SearchDeliveries(
            string supplierSearchTerm, string orderNumberSearchTerm, bool includeAcknowledged);
    }
}
