namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    public class MiniOrder
    {
        public int OrderNumber { get; set; }

        public DateTime? AdvisedDeliveryDate { get; set; }

        public string AcknowledgeComment { get; set; }
    }
}

