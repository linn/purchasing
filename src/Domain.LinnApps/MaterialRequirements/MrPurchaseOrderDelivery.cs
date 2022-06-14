namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System;

    public class MrPurchaseOrderDelivery
    {
        public string JobRef { get; set; }

        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySequence { get; set; }

        public decimal Quantity { get; set; }

        public decimal QuantityReceived { get; set; }

        public DateTime? RequestedDeliveryDate { get; set; }
        
        public DateTime? AdvisedDeliveryDate { get; set; }

        public string Reference { get; set; }
    }
}
