namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    public class PurchaseOrderDeliveryHistory
    {
        public DateTime? DateRequested { get; set; }

        public int DeliverySeq { get; set; }

        public int OrderLine { get; set; }

        public int OrderNumber { get; set; }

        public decimal? OurDeliveryQty { get; set; }

        public int HistoryNumber { get; set; }
    }
}
