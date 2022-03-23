namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    public class PurchaseOrderDelivery
    {
        public string Cancelled { get; set; }

        public DateTime? DateAdvised { get; set; }

        public DateTime? DateRequested { get; set; }

        public int DeliverySeq { get; set; }

        public decimal? NetTotal { get; set; }

        public int? OrderDeliveryQty { get; set; }

        public int OrderLine { get; set; }

        public int OrderNumber { get; set; }

        public int? OurDeliveryQty { get; set; }

        public PurchaseOrderDetail PurchaseOrderDetail { get; set; }

        public int? QtyNetReceived { get; set; }

        public decimal? QuantityOutstanding { get; set; }

        public DateTime? CallOffDate { get; set; }

        public decimal? BaseOurUnitPrice { get; set; }
    }
}
