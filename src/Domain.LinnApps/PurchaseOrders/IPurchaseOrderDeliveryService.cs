namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Keys;

    public interface IPurchaseOrderDeliveryService
    {
        IEnumerable<PurchaseOrderDelivery> SearchDeliveries(
            string supplierSearchTerm,
            string orderNumberSearchTerm,
            bool includeAcknowledged,
            int? orderLine = null);

        PurchaseOrderDelivery UpdateDelivery(
            PurchaseOrderDeliveryKey key,
            PurchaseOrderDelivery from,
            PurchaseOrderDelivery to,
            IEnumerable<string> privileges);

        BatchUpdateProcessResult BatchUpdateDeliveries(
            IEnumerable<PurchaseOrderDeliveryUpdate> changes,
            IEnumerable<string> privileges,
            bool skipSplitDeliveries = false);

        IEnumerable<PurchaseOrderDelivery> UpdateDeliveriesForOrderLine(
            int orderNumber,
            int orderLine,
            IEnumerable<PurchaseOrderDelivery> updated,
            IEnumerable<string> privileges);

        void UpdateMiniOrderDeliveries(IEnumerable<PurchaseOrderDelivery> updated);
    }
}
