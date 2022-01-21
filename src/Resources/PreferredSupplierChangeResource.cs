namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class PreferredSupplierChangeResource : HypermediaResource
    {
        public string PartNumber { get; set; }

        public int? Seq { get; set; }

        public int? OldSupplierId { get; set; }

        public string OldSupplierName { get; set; }

        public int? NewSupplierId { get; set; }

        public string NewSupplierName { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? NewPrice { get; set; }

        public string CurrencyCode { get; set; }

        public string DateChanged { get; set; }

        public int? ChangedById { get; set; }

        public string ChangedByName { get; set; }

        public string Remarks { get; set; }

        public string ChangeReasonCode { get; set; }

        public string ChangeReasonDescription { get; set; }
    }
}
