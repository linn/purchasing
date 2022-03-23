namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class PurchaseOrderDetail
    {
        public string Cancelled { get; set; }

        public int Line { get; set; }

        public decimal BaseNetTotal { get; set; }

        public decimal NetTotalCurrency { get; set; }

        public int OrderNumber { get; set; }

        public int? OurQty { get; set; }

        public Part Part { get; set; }

        public IEnumerable<PurchaseOrderDelivery> PurchaseDeliveries { get; set; }

        public string RohsCompliant { get; set; }

        public string SuppliersDesignation { get; set; }

        public string StockPoolCode { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
