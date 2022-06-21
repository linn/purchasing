namespace Linn.Purchasing.Resources
{
    using System;

    public class PurchaseOrderDeliveryUpdateResource
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySequence { get; set; }

        public string Reason { get; set; }

        public DateTime DateRequested { get; set; }

        public DateTime? DateAdvised { get; set; }
    }
}
