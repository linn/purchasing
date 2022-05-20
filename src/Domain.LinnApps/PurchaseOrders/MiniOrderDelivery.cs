namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    public class MiniOrderDelivery
    {
        public int OrderNumber { get; set; }

        public int DeliverySequence { get; set; }

        public DateTime? AdvisedDate { get; set; }
    }
}
