namespace Linn.Purchasing.Domain.LinnApps
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PurchaseOrder
    {
        public IEnumerable<PurchaseOrderDetail> Details { get; set; }

        public string DocumentType { get; set; }

        public int OrderNumber { get; set; }

        public Supplier Supplier { get; set; }

        public int SupplierId { get; set; }
    }
}
