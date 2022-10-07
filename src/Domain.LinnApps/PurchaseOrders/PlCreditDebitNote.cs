namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PlCreditDebitNote
    {
        public int NoteNumber { get; set; }

        public string PartNumber { get; set; }

        public decimal OrderQty { get; set; }

        public int? ReturnsOrderNumber { get; set; }

        public int? ReturnsOrderLine { get; set; }

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

        public Currency Currency { get; set; }

        public decimal? VatRate { get; set; }

        public int? CancelledBy { get; set; }

        public DateTime? DateCancelled { get; set; }

        public string ReasonCancelled { get; set; }

        public CreditDebitNoteType NoteType { get; set; }

        public string CreditOrReplace { get; set; }

        public int? OriginalOrderNumber { get; set; }

        public ICollection<PlCreditDebitNoteDetail> Details { get; set; }

        public int CreatedBy { get; set; }
    }
}
