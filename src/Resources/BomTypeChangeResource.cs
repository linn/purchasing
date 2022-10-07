namespace Linn.Purchasing.Resources
{
    public class BomTypeChangeResource
    {
        public string PartNumber { get; set; }

        public string PartDescription { get; set; }

        public string PartBomType { get; set; }

        public int? PreferredSuppliedId { get; set; }

        public decimal? PartCurrencyUnitPrice { get; set; }

        public decimal? PartBaseUnitPrice { get; set; }

        public string PartCurrency { get; set; }

        public int? OldSupplierId { get; set; }

        public int? NewSupplierId { get; set; }

        public string OldBomType { get; set; }

        public string NewBomType { get; set; }
    }
}
