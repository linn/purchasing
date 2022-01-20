namespace Linn.Purchasing.Domain.LinnApps.Parts
{
    using System;

    public class PartHistoryEntry
    {
        public string PartNumber { get; set; }

        public int Seq { get; set; }

        public int? OldLabourPrice { get; set; }

        public int? OldMaterialPrice { get; set; }

        public int? NewMaterialPrice { get; set; }

        public int? NewLabourPrice { get; set; }

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

        public int? NewCurrencyUnitPrice { get; set; }

        public int? OldCurrencyUnitPrice { get; set; }

        public int? OldBaseUnitPrice { get; set; }

        public int? NewBaseUnitPrice { get; set; }
    }
}
