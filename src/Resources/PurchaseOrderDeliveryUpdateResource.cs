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

        public decimal Qty { get; set; }

        public string Comment { get; set; }

        public string AvailableAtSupplier { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
