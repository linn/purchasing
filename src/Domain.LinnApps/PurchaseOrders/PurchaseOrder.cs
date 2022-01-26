namespace Linn.Purchasing.Domain.LinnApps
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrder
    {
        public string Cancelled { get; set; }

        public IEnumerable<PurchaseOrderDetail> Details { get; set; }

        public string DocumentType { get; set; }

        public DateTime OrderDate { get; set; }

        public int OrderNumber { get; set; }

        public Supplier Supplier { get; set; }

        public int SupplierId { get; set; }
    }
}
