namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PlCreditDebitNote
    {
        public int NoteNumber { get; set; }

        public string NoteType { get; set; }

        public string PartNumber { get; set; }

        public int OrderQty { get; set; }

        public int? ReturnsOrderNumber { get; set; }

        public decimal NetTotal { get; set; }

        public decimal Total { get; set; }

        public decimal OrderUnitPrice { get; set; }

        public string OrderUnitOfMeasure { get; set; }

        public decimal VatTotal { get; set; }

        public string Notes { get; set; }

        public DateTime? DateClosed { get; set; }

        public DateTime DateCreated { get; set; }

        public int? ClosedBy { get; set; }

        public string ReasonClosed { get; set; }

        public Supplier Supplier { get; set; }

        public string SuppliersDesignation { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
