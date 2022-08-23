namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using System;

    public class ReceiptPrefSupDiff
    {
        public int PlReceiptId { get; set; }

        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public DateTime DateBooked { get; set; }

        public decimal Qty { get; set; }

        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string PreferredSupplier { get; set; }

        public decimal CurrencyUnitPrice { get; set; }

        public string OrderCurrency { get; set; }

        public decimal ReceiptBaseUnitPrice { get; set; }

        public decimal PrefsupCurrencyUnitPrice { get; set; }

        public string PrefsupCurrency { get; set; }

        public decimal PrefsupBaseUnitPrice { get; set; }

        public decimal Difference { get; set; }

        public string MPVReason { get; set; }
    }
}
