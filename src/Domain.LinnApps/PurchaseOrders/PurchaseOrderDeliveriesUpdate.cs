namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public class PurchaseOrderDeliveriesUpdate
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        private IEnumerable<PurchaseOrderDeliveryUpdate> Deliveries { get; set;  }
    }
}
