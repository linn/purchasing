namespace Linn.Purchasing.Domain.LinnApps.Parts
{
    using System;

    public class PartHistoryEntry
    {
        public string PartNumber { get; set; }

        public int Seq { get; set; }

        public decimal? OldLabourPrice { get; set; }

        public decimal? OldMaterialPrice { get; set; }

        public decimal? NewMaterialPrice { get; set; }

        public decimal? NewLabourPrice { get; set; }

        public int? NewPreferredSupplierId { get; set; }

        public int? OldPreferredSupplierId { get; set; }

        public string OldBomType { get; set; }

        public string NewBomType { get; set; }

        public int ChangedBy { get; set; }

        public DateTime DateChanged { get; set; }

        public string ChangeType { get; set; }

        public string Remarks { get; set; }

        public string PriceChangeReason { get; set; }

        public string OldCurrency { get; set; }

        public string NewCurrency { get; set; }

        public decimal? NewCurrencyUnitPrice { get; set; }

        public decimal? OldCurrencyUnitPrice { get; set; }

        public decimal? OldBaseUnitPrice { get; set; }

        public decimal? NewBaseUnitPrice { get; set; }
    }
}
