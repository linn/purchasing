namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders
{
    using System;
    using System.Collections.Generic;

    public class MiniOrder
    {
        public int OrderNumber { get; set; }

        public DateTime? AdvisedDeliveryDate { get; set; }

        public string AcknowledgeComment { get; set; }

        public DateTime? RequestedDeliveryDate { get; set; }

        public int NumberOfSplitDeliveries { get; set; }

        public ICollection<MiniOrderDelivery> Deliveries { get; set; }

        public string SentByMethod { get; set; }
    }
}

