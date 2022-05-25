namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrder
{
    using System;

    public class MiniOrderDelivery
    {
        public int OrderNumber { get; set; }

        public int DeliverySequence { get; set; }

        public DateTime? AdvisedDate { get; set; }

        public DateTime? RequestedDate { get; set; }

        public MiniOrder Order { get; set; }

        public decimal? OurQty { get; set; }

        public string AvailableAtSupplier { get; set; }
    }
}
