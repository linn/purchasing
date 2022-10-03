namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    public class PlCreditDebitNoteDetail
    {
        public int NoteNumber { get; set; }

        public int LineNumber { get; set; }

        public string PartNumber { get; set; }

        public decimal OrderQty { get; set; }

        public int? OriginalOrderLine { get; set; } 

        public int? ReturnsOrderLine { get; set; }

        public decimal NetTotal { get; set; }

        public decimal Total { get; set; }

        public decimal OrderUnitPrice { get; set; }

        public string OrderUnitOfMeasure { get; set; }

        public decimal VatTotal { get; set; }

        public string Notes { get; set; }

        public string SuppliersDesignation { get; set; }

        public PlCreditDebitNote Header { get; set; }
    }
}
